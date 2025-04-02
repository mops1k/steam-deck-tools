using Launcher.Helper;
using Windows.Foundation;
namespace Launcher.Helper
{
    public static class ProcessActionCommandHelper
    {
        public delegate void ActionHandler(ToolManager manager, string tool);
        public static event ActionHandler? ProcessAction;

        public static int Execute(params string[] arguments)
        {
            var allowedTools = Enum.GetNames<Tools>();
            var tools = allowedTools.ToList();
            for (var i = 0; i < allowedTools.Length; i++)
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
            var toolManager = new ToolManager(processHelper, tools);
            foreach (var tool in tools)
            {
                ProcessAction?.Invoke(toolManager, tool);
            }

            return 0;
        }
    }
}
