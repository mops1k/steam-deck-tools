using CommonHelpers;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
namespace PowerControl.Helpers
{
    public static class SteamControllerKiller
    {
        public static bool IsRunning
        {
            get
            {
                return ProcessId != null;
            }
        }
        
        private static string? CurrentProcessDir
        {
            get {
                var currentProcess = Process.GetCurrentProcess();
                
                return Path.GetDirectoryName(currentProcess.MainModule?.FileName);
            }
        }

        private static int? ProcessId
        {
            get {
                var processes = Process.GetProcessesByName("SteamController");
                foreach (var process in processes)
                {
                    return process.Id;
                }

                return null;
            }
        }

        public static void ToggleSteamController()
        {
            if (SteamConfiguration.IsRunning && IsRunning)
            {
                KillSteamController();
            }

            if (!SteamConfiguration.IsRunning && !IsRunning)
            {
                RunSteamController();
            }
        }

        public static bool SteamControllerIsRunning()
        {
            return ProcessId != null;
        }

        public static bool RunSteamController()
        {
            var id = ProcessId;
            if (id != null)
            {
                return true;
            }

            var process = new Process();
            var dir = CurrentProcessDir;
            if (dir == null)
            {
                return false;
            }

            process.StartInfo.FileName = Path.Combine(dir, "SteamController.exe");
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            return process.Start();
        }

        public static bool KillSteamController()
        {
            var id = ProcessId;
            if (id == null)
            {
                return true;
            }

            try
            {
                var process = Process.GetProcessById((int)id);
                process.Kill();
                process.WaitForExit();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
