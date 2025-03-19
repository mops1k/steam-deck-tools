using CommonHelpers;
namespace Launcher.Helper
{
    public interface IToolManager
    {
        void StartTools();
        void StopTools(string? toolToStop = null);
    }
    
    public class ToolManager(IProcessHelper processHelper, string[] toolsToRun) : IToolManager
    {
        public void StartTools()
        {
            foreach (var tool in toolsToRun)
            {
                if (processHelper.IsToolRunning(tool))
                {
                    Log.Info($"{tool} is already running.");
                    continue;
                }

                Log.Info(processHelper.RunTool(tool, AppContext.BaseDirectory) ? $"{tool} started." : $"Failed to start {tool}.");
            }
        }

        public void StopTools(string? toolToStop = null)
        {
            var tools = string.IsNullOrEmpty(toolToStop) ? toolsToRun : [toolToStop];

            foreach (var tool in tools)
            {
                Log.Info(processHelper.StopTool(tool) ? $"{tool} stopped." : $"{tool} is not running.");
            }
        }
    }
}
