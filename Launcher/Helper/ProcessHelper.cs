using CommonHelpers;
using System.Diagnostics;
namespace Launcher.Helper
{
    public interface IProcessHelper
    {
        bool RunTool(string name, string directory);
        bool StopTool(string name);
        bool IsToolRunning(string name);
    }
    
    public class ProcessHelper : IProcessHelper
    {
        public bool RunTool(string name, string directory)
        {
            var process = new Process();
            process.StartInfo.FileName = Path.Combine(directory, name + ".exe");
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            try
            {
                return process.Start();
            }
            catch (Exception ex)
            {
                Log.Fatal($"Failed to start {name}.", ex);
                return false;
            }
        }

        public bool StopTool(string name)
        {
            var processes = Process.GetProcessesByName(name);
            if (processes.Length == 0)
            {
                Log.Info($"{name} is not running.");
                return false;
            }

            bool allStopped = true;

            foreach (var process in processes)
            {
                try
                {
                    // Мягкое завершение
                    if (process.CloseMainWindow())
                    {
                        Log.Info($"Sent close signal to {name} (PID: {process.Id}). Waiting for exit...");

                        if (process.WaitForExit(5000))
                        {
                            Log.Info($"{name} (PID: {process.Id}) closed gracefully.");
                            continue;
                        }
                    }

                    // Принудительное завершение
                    Log.Info($"{name} (PID: {process.Id}) did not close gracefully. Forcing termination...");
                    process.Kill();
                    process.WaitForExit();
                    Log.Info($"{name} (PID: {process.Id}) terminated forcefully.");
                }
                catch (Exception ex)
                {
                    Log.Fatal($"Failed to stop {name} (PID: {process.Id}).", ex);
                    allStopped = false;
                }
            }

            return allStopped;
        }

        public bool IsToolRunning(string name)
        {
            var processes = Process.GetProcessesByName(name);
            return processes.Length > 0;
        }
    }
}
