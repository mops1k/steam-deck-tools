using Launcher.Helper;
namespace Launcher.Command
{
    public class StopCommand(): AbstractCommand("stop")
    {
        public override int Run(params string[] arguments)
        {
            ProcessActionCommandHelper.ProcessAction += (manager, tool) => manager.StopTools(tool);

            return ProcessActionCommandHelper.Execute(arguments);
        }
    }
}
