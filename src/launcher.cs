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

        // -wait-for-browser keeps zen.exe running until the browser closes, so we
        // know when it is safe to clean up. If a browser from this folder is already
        // open, Zen hands the request (a new window or a link) to it and returns at once.
        StringBuilder cmd = new StringBuilder();
        cmd.Append("-wait-for-browser -profile ").Append(Quote(profileDir));
        foreach (string a in passThrough)
            cmd.Append(' ').Append(Quote(a));

        Process zen;
        try
        {
            zen = Process.Start(new ProcessStartInfo
            {
                FileName = zenExe,
                Arguments = cmd.ToString(),
                WorkingDirectory = Path.GetDirectoryName(zenExe),
                UseShellExecute = false
            });
        }
        catch (Exception ex)
        {
            ShowError("Zen Browser could not be started.\n\n" + ex.Message);
            return 1;
        }

        if (zen != null)
        {
            zen.WaitForExit();
            // Helper processes can take a moment to close. If the browser is still
            // open after that (this start only handed a link to it), the launcher
            // that started it will clean up later.
            string appDir = Path.GetDirectoryName(zenExe);
            for (int i = 0; i < 30; i++)
            {
                if (!IsBrowserRunningFrom(appDir))
                {
                    RegistryCleanup.RemoveTraces(appDir);
                    break;
                }
                System.Threading.Thread.Sleep(500);
            }
        }
        return 0;
    }

    static bool IsBrowserRunningFrom(string appDir)
    {
        string prefix = appDir.TrimEnd('\\') + "\\";
        foreach (Process p in Process.GetProcessesByName("zen"))
        {
            try
            {
                if (p.MainModule.FileName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            catch { } // processes we cannot inspect belong to someone else
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

    // A new profile gets the same portable defaults (user.js) as profile1.
    static void PrepareProfile(string dataDir, string profileDir)
    {
        if (Directory.Exists(profileDir)) return;
        Directory.CreateDirectory(profileDir);
        string template = Path.Combine(Path.Combine(dataDir, DefaultProfile), "user.js");
        if (File.Exists(template))
            File.Copy(template, Path.Combine(profileDir, "user.js"));
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

    static void DeleteIfEmpty(Microsoft.Win32.RegistryKey parent, string subPath)
    {
        using (Microsoft.Win32.RegistryKey key = parent.OpenSubKey(subPath))
        {
            if (key == null || key.SubKeyCount > 0 || key.ValueCount > 0) return;
        }
        parent.DeleteSubKey(subPath, false);
    }
}
