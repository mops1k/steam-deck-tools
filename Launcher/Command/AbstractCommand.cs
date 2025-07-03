namespace Launcher.Command
{
    public abstract class AbstractCommand(string fullName, string? shortName = null) : ICommand
    {
        private string FullName { get; } = fullName;
        private string? ShortName { get; } = shortName;

        public string GetFullName()
        {
            return FullName;
        }

        public string? GetShortName()
        {
            return ShortName;
        }

        public abstract int Run(params string[] arguments);
    }
}
