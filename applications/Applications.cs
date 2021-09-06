using System;
using System.Collections.Generic;
using System.IO;

namespace FastSetup.applications
{
    public static class Applications
    {
        private static List<Application> allApplications = new List<Application>();
        private static List<string> allApplicationIDs = new List<string>();

        public static List<Application> GetAllApplications()
        {
            return allApplications;
        }

        public static List<string> GetAllApplicationIDs()
        {
            return allApplicationIDs;
        }

        public static void RegisterApplication(Application application)
        {
            // Check for duplicate IDs
            if (allApplicationIDs.Contains(application.id))
            {
                throw new Exception($"Application with id {application.id} already exists");
            }

            allApplications.Add(application);
            allApplicationIDs.Add(application.id);
        }

        public static void AutoRegister()
        {
            // Register all applications with config in /data/apps
            foreach (string filePath in Directory.GetFiles(FastSetup.APPS_DIRECTORY, "*.json"))
            {
                RegisterApplication(new Application(Path.GetFileName(filePath)));
            }
        }
    }
}