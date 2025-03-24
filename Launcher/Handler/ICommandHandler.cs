namespace Launcher.Handler
{
    public interface ICommandHandler
    {
        public string GetFullName();
        public string? GetShortName();

        public int Run(params string[] arguments);
    }
}
