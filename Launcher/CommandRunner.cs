using Launcher.Command;
namespace Launcher
{
    public class CommandRunner
    {
        private readonly List<ICommand> _handlers = [];
        private ICommand? _isDefault;
        public void Run(string[] args)
        {
            var handler = GetCommandHandler(args);

            Environment.Exit(handler?.Run(args.Skip(1).ToArray()) ?? -1);
        }

        public CommandRunner RegisterCommand(ICommand handler, bool isDefault = false)
        {
            if (_handlers.Contains(handler))
            {
                return this;
            }

            _handlers.Add(handler);
            if (isDefault)
            {
                _isDefault = handler;
            }

            return this;
        }

        private ICommand? GetCommandHandler(string[] args)
        {
            if (args.Length == 0)
            {
                return _isDefault ?? null;
            }
            
            foreach (var handler in _handlers)
            {
                if (args[0].Contains($"--{handler.GetFullName()}"))
                {
                    return handler;
                }

                var shortName = handler.GetShortName();
                if (!String.IsNullOrEmpty(shortName) && args[0].Contains($"-{shortName}"))
                {
                    return handler;
                }
            }

            return null;
        }
    }
}
