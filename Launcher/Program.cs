using CommonHelpers;
using System.Diagnostics;
namespace Launcher
{
    internal static class Program
    {
        private static string? CurrentProcessDir
        {
            get {
                var currentProcess = Process.GetCurrentProcess();

                return Path.GetDirectoryName(currentProcess.MainModule?.FileName);
            }
        }
    
        private readonly static string[] _toolsToRun = [
            "FanControl",
            "PerfomanceOverlay",
            "SteamController",
            "PowerControl"
        ];

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run();
        
            try
            {
                foreach (var tool in _toolsToRun)
                {
                    RunTool(tool);
                }
            }
            catch (Exception e)
            {
                Log.Fatal("Process start fail", e);
            }
            finally
            {
                Application.Exit();
            }
        }

        private static void RunTool(string name)
        {
            if (IsToolRunned(name))
            {
                return;
            }
        
            var process = new Process();
            var dir = CurrentProcessDir;
            if (dir == null)
            {
                return;
            }
            
            process.StartInfo.FileName = Path.Combine(dir, name+".exe");
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            process.Start();
        }

        private static bool IsToolRunned(string name)
        {
            var processes = Process.GetProcessesByName(name);
        
            return processes.Length > 0;
        }
    }
}
