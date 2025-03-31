using CommonHelpers;
namespace Launcher.Helper
{
    public interface IToolManager
    {
        void StartTools(string? name = null);
        void StopTools(string? name = null);
        void RestartTools(string? name = null);
    }

    public class ToolManager(IProcessHelper processHelper, List<string> toolsToRun) : IToolManager
    {
        public void StartTools(string? name = null)
        {
            if (!String.IsNullOrEmpty(name) && !toolsToRun.Contains(name))
            {
                Log.Info($"Tool \"{name}\" does not exist.");
                
                Notification.ShowNotification($"Tool {name} does not exist.");
                return;
            }
            
            var tools = String.IsNullOrEmpty(name) ? toolsToRun : [name];

            foreach (var tool in tools)
            {
                if (processHelper.IsToolRunning(tool))
                {
                    Log.Info($"{tool} is already running.");
                    continue;
                }

                if (processHelper.RunTool(tool, AppContext.BaseDirectory))
                {
                    Log.Info($"{tool} started.");
                }
                else
                {
                    Log.Info($"Failed to start {tool}.");
                    Notification.ShowNotification($"Failed to start {tool}.");
                }
            }
        }

        public void StopTools(string? name = null)
        {
            if (!String.IsNullOrEmpty(name) && !toolsToRun.Contains(name))
            {
                Log.Info($"Tool \"{name}\" does not exist.");
                Notification.ShowNotification($"Tool \"{name}\" does not exist.");
                return;
            }

            var tools = string.IsNullOrEmpty(name) ? toolsToRun : [name];

            foreach (var tool in tools)
            {
                Log.Info(processHelper.StopTool(tool) ? $"{tool} stopped." : $"{tool} is not running.");
            }
        }
        
        public void RestartTools(string? name = null)
        {
            StopTools(name);
            StartTools(name);
        }

        public bool IsToolRunning(string name)
        {
            return processHelper.IsToolRunning(name);
        }
    }
}
