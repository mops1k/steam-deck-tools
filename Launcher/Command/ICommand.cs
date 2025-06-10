namespace Launcher.Command
{
    public interface ICommand
    {
        public string GetFullName();
        public string? GetShortName();

        public int Run(params string[] arguments);
    }
}
