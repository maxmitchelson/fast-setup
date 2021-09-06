using System.Collections.Generic;
using System.Diagnostics;

namespace FastSetup.tasks
{
    public class DesktopIconsTask : Task
    {

        public DesktopIconsTask(string baseConfigFile) : base("desktop_icons", baseConfigFile)
        {
        }

        public override void Start()
        {
            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "rundll32",
                    Arguments = "shell32.dll,Control_RunDLL desk.cpl,,0",
                    WorkingDirectory = @"C:\Windows\System32",
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    RedirectStandardOutput = true,
                }
            };

            process.Start();
        }

        public override List<string> GetCustomConfigKeys()
        {
            return null;
        }
    }
}