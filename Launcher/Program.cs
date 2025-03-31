using CommonHelpers;
using Launcher.Command;

namespace Launcher
{
    internal static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            Instance.UninstallTrigger();

            if (args.Length == 0)
            {
                ApplicationConfiguration.Initialize();
                Application.EnableVisualStyles();
                Application.Run(new LauncherForm());
                return;
            }

            new CommandRunner()
                .RegisterCommand(new StartCommand())
                .RegisterCommand(new StopCommand())
                .RegisterCommand(new RestartCommand())
                .RegisterCommand(new GenerateLinksCommand())
                .Run(args);
        }
    }
}
