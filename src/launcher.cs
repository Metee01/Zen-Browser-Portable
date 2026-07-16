using System;
using System.Diagnostics;
using System.IO;

class Program
{
    static void Main()
    {
        string dir = Path.GetDirectoryName(
            System.Reflection.Assembly.GetExecutingAssembly().Location);

        string zenExe = Path.Combine(dir, "app", "win", "zen.exe");
        string profile = Path.Combine(dir, "data", "profile1");

        if (!File.Exists(zenExe))
            return;

        Process.Start(new ProcessStartInfo
        {
            FileName = zenExe,
            Arguments = "-profile \"" + profile + "\" -no-remote",
            UseShellExecute = false,
            CreateNoWindow = true
        });
    }
}
