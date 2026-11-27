// Zen Browser Portable launcher
//
// Usage:
//   PortableZen.exe                         -> opens data\profile1
//   PortableZen.exe --profile 2             -> opens data\profile2
//   PortableZen.exe --profile work          -> opens data\work (created if missing)
//   PortableZen.exe https://example.com     -> opens the link in the portable browser
//   Hold Shift while starting               -> choose a profile from a list
//
// Any argument that is not --profile is passed to zen.exe unchanged, so links,
// files and "Open with" work as expected.
//
// Compile (no SDK needed, uses the C# compiler that ships with Windows):
//   C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe /nologo /target:winexe
//     /win32icon:zen-browser.ico /r:System.Windows.Forms.dll /r:System.Drawing.dll
//     /out:PortableZen.exe src\launcher.cs

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

static class Program
{
    const string Title = "Zen Browser Portable";
    const string DefaultProfile = "profile1";

    [STAThread]
    static int Main(string[] args)
    {
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string zenExe = Path.Combine(baseDir, @"app\win\zen.exe");
        string dataDir = Path.Combine(baseDir, "data");

        if (!File.Exists(zenExe))
        {
            ShowError("Zen Browser could not be found.\n\nExpected location:\n" + zenExe +
                "\n\nMake sure the \"app\\win\" folder next to PortableZen.exe contains the Zen Browser files.");
            return 1;
        }

        string requested = null;
        List<string> passThrough = new List<string>();
        for (int i = 0; i < args.Length; i++)
        {
            string a = args[i];
            bool isProfileFlag = a.Equals("--profile", StringComparison.OrdinalIgnoreCase) ||
                                 a.Equals("-p", StringComparison.OrdinalIgnoreCase);
            if (isProfileFlag && i + 1 < args.Length)
            {
                requested = args[++i];
                continue;
            }
            passThrough.Add(a);
        }

        if (requested == null && (Control.ModifierKeys & Keys.Shift) == Keys.Shift)
        {
            requested = PickProfile(dataDir);
            if (requested == null)
                return 0; // user cancelled the picker
        }

        string profileName = NormalizeProfileName(requested ?? DefaultProfile);
        if (profileName == null)
        {
            ShowError("Invalid profile name: " + requested +
                "\n\nUse a number (for example 2) or a simple folder name without slashes.");
            return 1;
        }

        string profileDir = Path.Combine(dataDir, profileName);
        try
        {
            PrepareProfile(dataDir, profileDir);
        }
        catch (Exception ex)
        {
            ShowError("The profile folder could not be created:\n" + profileDir + "\n\n" + ex.Message);
            return 1;
        }

        // If this portable Zen is already open, Zen hands the request (a new window
        // or a link) to the running browser, and the launcher that started that
        // browser is the one that cleans up when it closes.
        string appDir = Path.GetDirectoryName(zenExe);
        bool alreadyRunning = IsBrowserRunningFrom(appDir);

        StringBuilder cmd = new StringBuilder();
        cmd.Append("-profile ").Append(Quote(profileDir));
        foreach (string a in passThrough)
            cmd.Append(' ').Append(Quote(a));

        try
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = zenExe,
                Arguments = cmd.ToString(),
                WorkingDirectory = Path.GetDirectoryName(zenExe),
                UseShellExecute = false
            };
            // The crash reporter would store reports in %APPDATA%, outside the portable folder.
            psi.EnvironmentVariables["MOZ_CRASHREPORTER_DISABLE"] = "1";
            Process.Start(psi);
        }
        catch (Exception ex)
        {
            ShowError("Zen Browser could not be started.\n\n" + ex.Message);
            return 1;
        }

        if (alreadyRunning)
            return 0;

        // Zen may restart itself (after a version change or an update), so instead of
        // following one process we wait until nothing from app\win runs any more,
        // twice in a row a few seconds apart.
        System.Threading.Thread.Sleep(5000);
        while (true)
        {
            while (IsBrowserRunningFrom(appDir))
                System.Threading.Thread.Sleep(2000);
            System.Threading.Thread.Sleep(4000);
            if (!IsBrowserRunningFrom(appDir))
                break;
        }
        RegistryCleanup.RemoveTraces(appDir);
        RegistryCleanup.RemoveUpdateFolder(appDir);
        return 0;
    }

    // True while zen.exe or updater.exe from this app\win folder is running.
    // updater.exe counts because the browser is briefly closed while Zen applies an update.
    static bool IsBrowserRunningFrom(string appDir)
    {
        string prefix = appDir.TrimEnd('\\') + "\\";
        foreach (string name in new[] { "zen", "updater" })
        {
            foreach (Process p in Process.GetProcessesByName(name))
            {
                string path = NativeMethods.GetImagePath(p.Id);
                p.Dispose();
                if (path != null && path.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
        }
        return false;
    }

    // "2" -> "profile2"; "work" -> "work". Rejects anything that could leave the data folder.
    static string NormalizeProfileName(string name)
    {
        if (name == null) return null;
        name = name.Trim();
        int number;
        if (int.TryParse(name, out number) && number > 0)
            return "profile" + number;
        if (name.Length == 0 || name == "." || name == ".." ||
            name.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 ||
            name.IndexOf('/') >= 0 || name.IndexOf('\\') >= 0)
            return null;
        return name;
    }

    // Portable defaults live in app\win\distribution\policies.json, so a new
    // profile only needs its folder.
    static void PrepareProfile(string dataDir, string profileDir)
    {
        Directory.CreateDirectory(profileDir);
    }

    static string PickProfile(string dataDir)
    {
        List<string> names = new List<string>();
        if (Directory.Exists(dataDir))
            foreach (string d in Directory.GetDirectories(dataDir))
                names.Add(Path.GetFileName(d));
        names.Sort(StringComparer.OrdinalIgnoreCase);
        if (names.Count == 0) names.Add(DefaultProfile);

        using (Form form = new Form())
        {
            form.Text = Title + " - choose a profile";
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.ClientSize = new Size(320, 260);
            try { form.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath); } catch { }

            ListBox list = new ListBox();
            list.SetBounds(12, 12, 296, 196);
            foreach (string n in names) list.Items.Add(n);
            list.SelectedIndex = 0;

            Button open = new Button();
            open.Text = "Open";
            open.SetBounds(152, 220, 75, 28);
            open.DialogResult = DialogResult.OK;

            Button cancel = new Button();
            cancel.Text = "Cancel";
            cancel.SetBounds(233, 220, 75, 28);
            cancel.DialogResult = DialogResult.Cancel;

            list.DoubleClick += delegate { form.DialogResult = DialogResult.OK; };
            form.AcceptButton = open;
            form.CancelButton = cancel;
            form.Controls.Add(list);
            form.Controls.Add(open);
            form.Controls.Add(cancel);

            if (form.ShowDialog() != DialogResult.OK || list.SelectedItem == null)
                return null;
            return (string)list.SelectedItem;
        }
    }

    // Quotes one argument using the rules CommandLineToArgvW understands.
    static string Quote(string arg)
    {
        if (arg.Length > 0 && arg.IndexOfAny(new[] { ' ', '\t', '"' }) < 0)
            return arg;
        StringBuilder sb = new StringBuilder("\"");
        int backslashes = 0;
        foreach (char c in arg)
        {
            if (c == '\\') { backslashes++; continue; }
            if (c == '"') sb.Append('\\', backslashes * 2 + 1);
            else sb.Append('\\', backslashes);
            backslashes = 0;
            sb.Append(c);
        }
        sb.Append('\\', backslashes * 2);
        sb.Append('"');
        return sb.ToString();
    }

    static void ShowError(string message)
    {
        MessageBox.Show(message, Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}

// Zen (like Firefox) writes a few small values to HKCU\Software\Mozilla\Zen while it
// runs: window size for the startup screen, launch times, default-browser-agent
// settings. Their names start with the folder Zen was started from, so we remove
// exactly those and leave everything else (for example an installed Zen) untouched.
static class RegistryCleanup
{
    const string ZenKey = @"Software\Mozilla\Zen";

    public static void RemoveTraces(string appDir)
    {
        try
        {
            string prefix = appDir.TrimEnd('\\');
            using (Microsoft.Win32.RegistryKey zenKey =
                Microsoft.Win32.Registry.CurrentUser.OpenSubKey(ZenKey, true))
            {
                if (zenKey == null) return;
                foreach (string sub in zenKey.GetSubKeyNames())
                {
                    using (Microsoft.Win32.RegistryKey key = zenKey.OpenSubKey(sub, true))
                    {
                        if (key == null) continue;
                        foreach (string name in key.GetValueNames())
                        {
                            if (name.StartsWith(prefix + "\\", StringComparison.OrdinalIgnoreCase) ||
                                name.StartsWith(prefix + "|", StringComparison.OrdinalIgnoreCase))
                                key.DeleteValue(name, false);
                        }
                    }
                    DeleteIfEmpty(zenKey, sub);
                }
            }
            DeleteIfEmpty(Microsoft.Win32.Registry.CurrentUser, ZenKey);
            DeleteIfEmpty(Microsoft.Win32.Registry.CurrentUser, @"Software\Mozilla");
        }
        catch { } // cleanup is best effort and must never bother the user
    }

    // Zen also keeps a small update folder per install location under
    // C:\ProgramData\Mozilla-<GUID>\updates\<hash>, where <hash> is CityHash64
    // (v1.0) of the install folder path in UTF-16. We remove only our own one.
    public static void RemoveUpdateFolder(string appDir)
    {
        try
        {
            string path = appDir.TrimEnd('\\');
            string hash = CityHash.Hash64(System.Text.Encoding.Unicode.GetBytes(path)).ToString("X16");
            string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                @"Mozilla-1de4eec8-1241-4177-a864-e594e8d1fb38\updates\" + hash);
            if (Directory.Exists(dir))
                Directory.Delete(dir, true);
        }
        catch { }
    }

    static void DeleteIfEmpty(Microsoft.Win32.RegistryKey parent, string subPath)
    {
        using (Microsoft.Win32.RegistryKey key = parent.OpenSubKey(subPath))
        {
            if (key == null || key.SubKeyCount > 0 || key.ValueCount > 0) return;
        }
        parent.DeleteSubKey(subPath, false);
    }
}

// Reads a process's executable path with the cheapest access right, so polling
// does not have to load module lists of unrelated processes.
static class NativeMethods
{
    const uint PROCESS_QUERY_LIMITED_INFORMATION = 0x1000;

    [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
    static extern IntPtr OpenProcess(uint access, bool inherit, int pid);

    [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
    static extern bool CloseHandle(IntPtr handle);

    [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
    static extern bool QueryFullProcessImageName(IntPtr process, int flags, StringBuilder name, ref int size);

    public static string GetImagePath(int pid)
    {
        IntPtr h = OpenProcess(PROCESS_QUERY_LIMITED_INFORMATION, false, pid);
        if (h == IntPtr.Zero) return null;
        try
        {
            StringBuilder sb = new StringBuilder(1024);
            int size = sb.Capacity;
            return QueryFullProcessImageName(h, 0, sb, ref size) ? sb.ToString(0, size) : null;
        }
        finally { CloseHandle(h); }
    }
}

// CityHash64 v1.0, the version Mozilla uses to name per-install folders.
static class CityHash
{
    const ulong K0 = 0xc3a5c85c97cb3127UL, K1 = 0xb492b66fbe98f273UL,
                K2 = 0x9ae16a3b2f90404fUL, K3 = 0xc949d7c7509e6557UL;

    static ulong F64(byte[] s, int i) { return BitConverter.ToUInt64(s, i); }
    static ulong F32(byte[] s, int i) { return BitConverter.ToUInt32(s, i); }
    static ulong Rot(ulong v, int n) { return n == 0 ? v : (v >> n) | (v << (64 - n)); }
    static ulong Rot1(ulong v, int n) { return (v >> n) | (v << (64 - n)); }
    static ulong ShiftMix(ulong v) { return v ^ (v >> 47); }

    static ulong Len16(ulong u, ulong v)
    {
        const ulong kMul = 0x9ddfea08eb382d69UL;
        ulong a = (u ^ v) * kMul; a ^= a >> 47;
        ulong b = (v ^ a) * kMul; b ^= b >> 47;
        return b * kMul;
    }

    static ulong Len0to16(byte[] s, int n)
    {
        if (n > 8)
        {
            ulong a = F64(s, 0), b = F64(s, n - 8);
            return Len16(a, Rot1(b + (ulong)n, n)) ^ b;
        }
        if (n >= 4)
            return Len16((ulong)n + (F32(s, 0) << 3), F32(s, n - 4));
        if (n > 0)
        {
            ulong y = (ulong)s[0] + ((ulong)s[n >> 1] << 8);
            ulong z = (ulong)n + ((ulong)s[n - 1] << 2);
            return ShiftMix(y * K2 ^ z * K3) * K2;
        }
        return K2;
    }

    static ulong Len17to32(byte[] s, int n)
    {
        ulong a = F64(s, 0) * K1, b = F64(s, 8), c = F64(s, n - 8) * K2, d = F64(s, n - 16) * K0;
        return Len16(Rot(a - b, 43) + Rot(c, 30) + d, a + Rot(b ^ K3, 20) - c + (ulong)n);
    }

    static ulong Len33to64(byte[] s, int n)
    {
        ulong z = F64(s, 24);
        ulong a = F64(s, 0) + ((ulong)n + F64(s, n - 16)) * K0;
        ulong b = Rot(a + z, 52), c = Rot(a, 37);
        a += F64(s, 8); c += Rot(a, 7); a += F64(s, 16);
        ulong vf = a + z, vs = b + Rot(a, 31) + c;
        a = F64(s, 16) + F64(s, n - 32);
        z = F64(s, n - 8);
        b = Rot(a + z, 52); c = Rot(a, 37);
        a += F64(s, n - 24); c += Rot(a, 7); a += F64(s, n - 16);
        ulong wf = a + z, ws = b + Rot(a, 31) + c;
        ulong r = ShiftMix((vf + ws) * K2 + (wf + vs) * K0);
        return ShiftMix(r * K0 + vs) * K2;
    }

    static void Weak32(byte[] s, int i, ulong a, ulong b, out ulong first, out ulong second)
    {
        ulong w = F64(s, i), x = F64(s, i + 8), y = F64(s, i + 16), z = F64(s, i + 24);
        a += w; b = Rot(b + a + z, 21);
        ulong c = a; a += x; a += y; b += Rot(a, 44);
        first = a + z; second = b + c;
    }

    public static ulong Hash64(byte[] s)
    {
        int n = s.Length;
        if (n <= 16) return Len0to16(s, n);
        if (n <= 32) return Len17to32(s, n);
        if (n <= 64) return Len33to64(s, n);

        ulong x = F64(s, 0), y = F64(s, n - 16) ^ K1, z = F64(s, n - 56) ^ K0;
        ulong v1, v2, w1, w2;
        Weak32(s, n - 64, (ulong)n, y, out v1, out v2);
        Weak32(s, n - 32, (ulong)n * K1, K0, out w1, out w2);
        z += ShiftMix(v2) * K1;
        x = Rot(z + x, 39) * K1;
        y = Rot(y, 33) * K1;

        int len = (n - 1) & ~63, i = 0;
        do
        {
            x = Rot(x + y + v1 + F64(s, i + 16), 37) * K1;
            y = Rot(y + v2 + F64(s, i + 48), 42) * K1;
            x ^= w2;
            y ^= v1;
            z = Rot(z ^ w1, 33);
            Weak32(s, i, v2 * K1, x + w1, out v1, out v2);
            Weak32(s, i + 32, z + w2, y, out w1, out w2);
            ulong t = z; z = x; x = t;
            i += 64; len -= 64;
        } while (len != 0);
        return Len16(Len16(v1, w1) + ShiftMix(y) * K1 + z, Len16(v2, w2) + x);
    }
}
