using System.Collections.Generic;
using SimpleWifi;

namespace ConfigHelper.tasks
{
    public class ConnectWifiTask : Task
    {
        private Wifi wifi;
        private string ssid;
        private string password;
        private string username;
        private string domain;

        public ConnectWifiTask(string baseConfigFile) : base("connect_wifi", baseConfigFile)
        {
            this.ssid = this.settings.ssid;
            this.password = this.settings.password;
            this.username = this.settings.username;
            this.domain = this.settings.domain;

            this.wifi = new Wifi();
        }

        public override List<string> GetCustomConfigKeys()
        {
            return new List<string>() { "ssid" };
        }

        public override void Start()
        {
            ConfigHelper.AlertF("task.started", new Dictionary<string, string>()
                {
                    { "task_name", ConfigHelper.GetTranslation("task.connect_wifi.name") }
                }
            );

            AccessPoint selectedAP = GetSelectedAP();

            if (selectedAP != null)
            {
                AuthRequest authRequest = new AuthRequest(selectedAP);

                if (authRequest.IsPasswordRequired)
                {
                    if (this.password == null)
                    {
                        ConfigHelper.Alert("task.connect_wifi.missing_parameter.password");
                    }
                    else
                    {
                        if (selectedAP.IsValidPassword(this.password))
                        {
                            authRequest.Password = this.password;
                        }
                        else
                        {
                            ConfigHelper.Alert("task.connect_wifi.invalid_password");
                        }
                    }
                }

                if (authRequest.IsUsernameRequired)
                {
                    if (this.username == null)
                    {
                        ConfigHelper.Alert("task.connect_wifi.missing_parameter.username");
                    }
                    else
                    {
                        authRequest.Username = this.username;
                    }
                }

                if (authRequest.IsDomainSupported)
                {
                    if (this.domain == null)
                    {
                        ConfigHelper.Alert("task.connect_wifi.missing_parameter.domain");
                    }
                    else
                    {
                        authRequest.Domain = this.domain;
                    }
                }

                bool isSuccess = selectedAP.Connect(authRequest, !selectedAP.HasProfile);
                OnConnectionDone(isSuccess);
            }
            else
            {
                ConfigHelper.Alert("task.connect_wifi.failed.ap_not_found");
            }
        }

        private AccessPoint GetSelectedAP()
        {
            foreach (AccessPoint ap in this.wifi.GetAccessPoints())
            {
                if (ap.Name == this.ssid) return ap;
            }

            return null;
        }

        private void OnConnectionDone(bool isSuccess)
        {
            if (isSuccess)
            {
                ConfigHelper.Alert("task.connect_wifi.connected");
            }

            else
            {
                ConfigHelper.Alert("task.connect_wifi.failed.connection_failed");
            }
        }
    }
}