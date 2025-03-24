using CommonHelpers;
using Launcher.Handler;
using Launcher.Helper;
using System.Diagnostics;
namespace Launcher
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var runner = new CommandRunner();
            runner.RegisterHandler(new StartHandler(), true);
            runner.RegisterHandler(new StopHandler());
            runner.RegisterHandler(new RestartHandler());
            runner.RegisterHandler(new GenerateLinksHandler());

            runner.Run(args);
        }
    }
}
