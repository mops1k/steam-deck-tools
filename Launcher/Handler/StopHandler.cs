using Launcher.Helper;
namespace Launcher.Handler
{
    public class StopHandler: AbstractCommandHandler
    {
        public StopHandler()
        {
            base.FullName = "stop";
        }
        
        public override int Run(params string[] arguments)
        {
            var allowedTools = Enum.GetNames<Tools>();
            var tools = allowedTools.ToList();
            for (int i = 0; i < allowedTools.Length; i++)
            {
                allowedTools[i] = allowedTools[i].ToLower();
            }

            if (arguments.Length > 0)
            {
                if (!allowedTools.Contains(arguments[0].ToLower()))
                {
                    return -1;
                }

                tools = [arguments[0]];
            }

            var processHelper = new ProcessHelper();
            var toolsToRun = new[] { "FanControl", "PerformanceOverlay", "SteamController", "PowerControl" };
            var toolManager = new ToolManager(processHelper, toolsToRun);

            foreach (var tool in tools)
            {
                toolManager.StopTools(tool);
            }

            return 0;
        }
    }
}
