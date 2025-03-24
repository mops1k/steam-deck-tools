using Launcher.Helper;
namespace Launcher.Handler
{
    public class GenerateLinksHandler: AbstractCommandHandler
    {
        public GenerateLinksHandler()
        {
            base.FullName = "generate-links";
            base.ShortName = "gl";
        }
        public override int Run(params string[] arguments)
        {
            var shortcutGenerator = new ShortcutGenerator();
            shortcutGenerator.GenerateShortcuts();

            return 0;
        }
    }
}
