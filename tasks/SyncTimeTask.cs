using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FastSetup.tasks
{
    public class SyncTimeTask : BaseTask
    {
        private string time_server;
        private string default_tz;
        private bool auto_tz;

        public SyncTimeTask(string baseConfigFile) : base("sync_time", baseConfigFile)
        {
            this.time_server = this.settings.time_server;
            this.auto_tz = this.settings.auto_tz;
            this.default_tz = this.settings.default_tz;
        }

        public override List<string> GetCustomConfigKeys()
        {
            return new List<string>() { "time_server", "auto_tz", "default_tz" };
        }

        public override void Start()
        {
            FastSetup.AlertF("task.started", new Dictionary<string, string>()
                {
                    { "task_name", FastSetup.GetTranslation("task.sync_time.name") }
                }
            );

            List<string> commands = new List<string> {
                // Register w32tm
                "w32tm /register",

                // Start time service
                "net start w32time",

                // Manually set time zone
                $"tzutil /s \"{this.default_tz}\"",

                // Manually set time server url
                $"w32tm /config /manualpeerlist:{this.time_server},0x1 /update",
                
                // Synchronize with time server
                "w32tm /resync"
            };

            if (this.auto_tz)
            {
                // Enable automatic time zones
                commands.Add("sc config tzautoupdate start= auto");
                commands.Add("net start tzautoupdate");
            }

            for (int i = 0; i < commands.Count; i++)
            {
                Console.WriteLine($"{i + 1}- {commands[i]}");
                Process process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = "/c " + commands[i],
                        Verb = "runas",
                        WorkingDirectory = @"C:\Windows\System32",
                        UseShellExecute = false,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        RedirectStandardOutput = true,
                    }
                };

                process.Start();
                process.WaitForExit();
            }

        }

    }
}