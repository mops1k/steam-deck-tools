namespace Launcher.Handler
{
    public abstract class AbstractCommandHandler(string fullName, string? shortName = null) : ICommandHandler
    {
        protected string FullName { get; init; } = fullName;
        protected string? ShortName { get; init; } = shortName;

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
