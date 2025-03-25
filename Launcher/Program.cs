using Launcher.Handler;

namespace Launcher
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new LauncherForm());
                return;
            }

            var runner = new CommandRunner();
            runner.RegisterHandler(new StartHandler());
            runner.RegisterHandler(new StopHandler());
            runner.RegisterHandler(new RestartHandler());
            runner.RegisterHandler(new GenerateLinksHandler());

            runner.Run(args);
        }
    }
}
