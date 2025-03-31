using Launcher.Helper;
namespace Launcher.Command
{
    public class GenerateLinksCommand() : AbstractCommand("generate-links", "gl")
    {
        public override int Run(params string[] arguments)
        {
            var shortcutGenerator = new ShortcutGenerator();
            shortcutGenerator.GenerateShortcuts();

            return 0;
        }
    }
}
