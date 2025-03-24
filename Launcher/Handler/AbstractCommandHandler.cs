namespace Launcher.Handler
{
    public abstract class AbstractCommandHandler : ICommandHandler
    {
        protected string FullName;
        protected string? ShortName = null;
        
        public string GetFullName()
        {
            return this.FullName;
        }

        public string? GetShortName()
        {
            return this.ShortName;
        }

        public abstract int Run(params string[] arguments);
    }
}
