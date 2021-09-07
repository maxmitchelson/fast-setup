using System.Collections.Generic;

namespace FastSetup.tasks
{
    public class Tasks
    {
        private static List<BaseTask> allTasks = new List<BaseTask>();
        private static List<string> allTaskIDs = new List<string>();


        static BaseTask ConnectWifi;
        static BaseTask SyncTime;
        static BaseTask DesktopIcons;


        public static List<BaseTask> GetAllTasks()
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

        public static BaseTask Register(BaseTask task)
        {
            allTasks.Add(task);
            allTaskIDs.Add(task.id);

            return task;
        }
    }
}