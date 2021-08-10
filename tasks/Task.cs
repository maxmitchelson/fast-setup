using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

namespace ConfigHelper.tasks
{
    public abstract class Task
    {
        public string id;
        public string baseConfigFile;
        public bool requiresAdmin;

        private dynamic jsonConfig;
        protected dynamic settings;

        private List<string> CUSTOM_CONFIG_KEYS;
        private static List<string> REQUIRED_CONFIG_KEYS = new List<string>() { "id", "requiresAdmin" };

        protected Task(string idIn, string baseConfigFileIn)
        {
            this.id = idIn;
            this.baseConfigFile = baseConfigFileIn;
            this.CUSTOM_CONFIG_KEYS = this.GetCustomConfigKeys();
            this.ImportAndValidateConfiguration();
        }

        private void ImportAndValidateConfiguration()
        {
            this.jsonConfig = this.ReadConfigFile();

            // Assert that all required keys are present in config file
            foreach (string key in REQUIRED_CONFIG_KEYS)
            {
                if (jsonConfig[key] == null)
                {
                    throw new ArgumentNullException($"{key} in tasks/{this.baseConfigFile}");

                }
            }

            // Assert that all custom keys are present in config file
            if (this.CUSTOM_CONFIG_KEYS != null)
            {
                if (jsonConfig["settings"] == null)
                {
                    throw new ArgumentNullException($"settings in tasks/{this.baseConfigFile}");

                }
                
                foreach (string key in this.CUSTOM_CONFIG_KEYS)
                {
                    if (jsonConfig.settings[key] == null)
                    {
                        throw new ArgumentNullException($"{key} in tasks/{this.baseConfigFile}");
                    }
                }
            }


            // Verify that the id matches with the config file
            if (this.id != (string)jsonConfig.id)
            {
                throw new Exception($"id \'{jsonConfig.id}\' does not match expected value \'{this.id}\' (tasks/{this.baseConfigFile})");
            }

            this.requiresAdmin = jsonConfig.requiresAdmin;
            this.settings = jsonConfig.settings;
        }

        protected JObject ReadConfigFile()
        {
            string configFilePath = Path.Combine(ConfigHelper.TASKS_DIRECTORY, baseConfigFile);

            if (File.Exists(configFilePath))
            {
                return JObject.Parse(File.ReadAllText(configFilePath));
            }
            else
            {
                throw new FileNotFoundException($"The config file was not found ({configFilePath})");
            }
        }

        public abstract void Start();

        public abstract List<string> GetCustomConfigKeys();
    }
}