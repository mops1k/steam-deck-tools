using Launcher.Helper;
namespace Launcher.Handler
{
    public class GenerateLinksHandler() : AbstractCommandHandler("generate-links", "gl")
    {
        public override int Run(params string[] arguments)
        {
            var shortcutGenerator = new ShortcutGenerator();
            shortcutGenerator.GenerateShortcuts();

            return 0;
        }
    }
}
