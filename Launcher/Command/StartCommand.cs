using Launcher.Helper;
namespace Launcher.Command
{
    public class StartCommand() : AbstractCommand("start")
    {
        public override int Run(params string[] arguments)
        {
            ProcessActionCommandHelper.ProcessAction += (manager, tool) => manager.StartTools(tool);

            return ProcessActionCommandHelper.Execute(arguments);
        }
    }
}
