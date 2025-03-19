using CommonHelpers;
using Launcher.Helper;
using System.Diagnostics;
namespace Launcher
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var processHelper = new ProcessHelper();
            var toolsToRun = new[] { "FanControl", "PerformanceOverlay", "SteamController", "PowerControl" };
            var toolManager = new ToolManager(processHelper, toolsToRun);
            var shortcutGenerator = new ShortcutGenerator();

            if (args.Contains("--generate-links"))
            {
                shortcutGenerator.GenerateShortcuts();
                return;
            }

            if (args.Contains("--stop-apps") || args.Contains("-s"))
            {
                string? toolToStop = args.Length > 1 ? args[1] : null;
                toolManager.StopTools(toolToStop);
            }
            else
            {
                toolManager.StartTools();
            }
        }
    }
}
