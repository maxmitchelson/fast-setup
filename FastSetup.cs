using System;
using System.Collections.Generic;
using System.IO;
using FastSetup.applications;
using FastSetup.tasks;

namespace FastSetup
{
    class FastSetup
    {
        // Paths to resources
        public static string BASE_DIRECTORY = Directory.GetCurrentDirectory();
        public static string DATA_DIRECTORY = Path.Combine(BASE_DIRECTORY, "data");

        public static string APPS_DIRECTORY = Path.Combine(DATA_DIRECTORY, "apps");
        public static string TASKS_DIRECTORY = Path.Combine(DATA_DIRECTORY, "tasks");
        public static string PROFILES_DIRECTORY = Path.Combine(DATA_DIRECTORY, "profiles");

        static void Main(string[] args)
        {
            Applications.AutoRegister();
            Tasks.RegisterTasks();
            // registerProfiles();

            foreach (Task task in Tasks.GetAllTasks())
            {
                task.Start();
            }
        }

        public static void Alert(string messageKey)
        {
            // TODO: alert system 
            string message = GetTranslation(messageKey);
            Console.WriteLine(message);
        }

        public static void AlertF(string messageKey, Dictionary<string, string> KeyValuePairsToFormat)
        {
            // TODO: formatted alert system
            string message = GetTranslation(messageKey);

            foreach (KeyValuePair<string, string> KeyValuePair in KeyValuePairsToFormat)
            {
                message = message.Replace("{" + KeyValuePair.Key + "}", KeyValuePair.Value);
            }

            Console.WriteLine(message);
        }

        public static string GetTranslation(string messageKey)
        {
            // TODO: translation system
            return messageKey;
        }
    }
}
