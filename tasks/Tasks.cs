using System.Collections.Generic;

namespace ConfigHelper.tasks
{
    public class Tasks
    {
        private static List<Task> allTasks = new List<Task>();
        private static List<string> allTaskIDs = new List<string>();


        static Task ConnectWifi;
        static Task SyncTime;
        static Task DesktopIcons;


        public static List<Task> GetAllTasks()
        {
            return allTasks;
        }

        public static List<string> GetAllTaskIDs()
        {
            return allTaskIDs;
        }

        public static void RegisterTasks()
        {
            ConnectWifi = Register(new ConnectWifiTask("connect_wifi.json"));
            SyncTime = Register(new SyncTimeTask("sync_time.json"));
            DesktopIcons = Register(new DesktopIconsTask("desktop_icons.json"));
        }

        public static Task Register(Task task)
        {
            allTasks.Add(task);
            allTaskIDs.Add(task.id);

            return task;
        }
    }
}