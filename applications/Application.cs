using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json.Linq;

namespace ConfigHelper.applications
{
    public class Application
    {
        public string baseConfigFile;
        public string id;
        public string name;

        public bool requiresNetwork;
        public bool requiresAdmin;
        public bool autoDeletes;

        public bool languageDependent;

        public Dictionary<string, string> paths = new Dictionary<string, string>();

        public Application(string baseConfigFileIn)
        {
            this.baseConfigFile = baseConfigFileIn;
            this.ImportConfiguration();
            this.VerifyValidConfiguration();
        }

        protected void ImportConfiguration()
        {
            dynamic jsonConfig = this.ReadConfigFile();
            this.id = jsonConfig.id;
            this.name = jsonConfig.name;
            this.requiresNetwork = jsonConfig.requiresNetwork;
            this.requiresAdmin = jsonConfig.requiresAdmin;
            this.autoDeletes = jsonConfig.autoDeletes;
            this.languageDependent = jsonConfig.languageDependent;
            this.paths = jsonConfig.paths.ToObject<Dictionary<string, string>>();

            // Change from relative paths to absolute paths
            List<string> pathLanguages = new List<string>(this.paths.Keys);
            foreach (string lang in pathLanguages)
            {
                string tempPath = Path.Combine(ConfigHelper.APPS_DIRECTORY, "bin");
                if (this.languageDependent)
                {
                    tempPath = Path.Combine(tempPath, lang);
                }

                this.paths[lang] = Path.Combine(tempPath, this.paths[lang]);
            }
        }

        protected void VerifyValidConfiguration()
        {
            // Verify that all required properties are defined
            Dictionary<string, object> allProperties = new Dictionary<string, object>
            {
                {"id", this.id},
                {"name", this.name},
                {"requiresNetwork", this.requiresNetwork},
                {"requiresAdmin", this.requiresAdmin},
                {"autoDeletes", this.autoDeletes},
                {"languageDependent", this.languageDependent},
                {"paths", this.paths}
            };

            foreach (string property in allProperties.Keys)
            {
                if (allProperties[property] == null)
                {
                    throw new ArgumentNullException($"{property} in apps/{this.baseConfigFile}");
                }
            }

            // Verify that all paths are valid
            foreach (string path in this.paths.Values)
            {
                if (!File.Exists(path))
                {
                    throw new FileNotFoundException($"\'{path}\' for app \'{this.name}\'");
                }
            }
        }

        protected JObject ReadConfigFile()
        {
            string configFilePath = Path.Combine(ConfigHelper.APPS_DIRECTORY, baseConfigFile);

            if (File.Exists(configFilePath))
            {
                return JObject.Parse(File.ReadAllText(configFilePath));
            }
            else
            {
                throw new FileNotFoundException($"The config file was not found ({configFilePath})");
            }
        }

        public void Install(string language)
        {
            if (!this.languageDependent) { language = "default"; }
            string filePath = this.paths[language];

            // Creates a temporary copy of the installer to prevent auto-deleting
            if (this.autoDeletes)
            {
                // Creates a 'temp' directory if it doesn't exist
                string tempDirectoryPath = Path.Combine(ConfigHelper.APPS_DIRECTORY, "temp");
                if (! Directory.Exists(tempDirectoryPath))
                {
                    Directory.CreateDirectory(tempDirectoryPath);
                }

                // Creates a temporary copy of the file if it doesn't exist
                string tempFilePath = Path.Combine(tempDirectoryPath, Path.GetFileName(this.paths[language]));
                if (! File.Exists(tempFilePath))
                {
                    File.Copy(this.paths[language], tempFilePath);
                }

                filePath = tempFilePath;
            }

            Process.Start(filePath);
        }
    }
}