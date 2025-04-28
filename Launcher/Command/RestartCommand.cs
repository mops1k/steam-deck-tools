using Launcher.Helper;
namespace Launcher.Command
{
    public class RestartCommand(): AbstractCommand("restart", "r")
    {
        public override int Run(params string[] arguments)
        {
            ProcessActionCommandHelper.ProcessAction += (manager, tool) => manager.RestartTools(tool);

            return ProcessActionCommandHelper.Execute(arguments);
        }
    }
}
