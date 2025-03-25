using Launcher.Handler;
namespace Launcher
{
    public class CommandRunner
    {
        private List<ICommandHandler> _handlers = [];
        private ICommandHandler? _isDefault = null;
        public int Run(string[] args)
        {
            var handler = GetCommandHandler(args);

            return handler?.Run(args.Skip(1).ToArray()) ?? -1;
        }

        public CommandRunner RegisterHandler(ICommandHandler handler, bool isDefault = false)
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

        private ICommandHandler? GetCommandHandler(string[] args)
        {
            if (args.Length == 0)
            {
                if (_isDefault != null)
                {
                    return _isDefault;
                }

                return null;
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
