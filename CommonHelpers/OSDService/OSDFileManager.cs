using System.Windows.Ink;

namespace CommonHelpers.OSDService
{
    public class OSDFileManager
    {
        private string Directory { get; }
        private readonly FileListProvider _fileListProvider;
        private readonly FileSaver _fileSaver = new FileSaver();
        private readonly FileLoader _fileLoader = new FileLoader();

        public OSDFileManager()
        {
            var dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            Directory = Path.Combine(dir, "SteamDeckTools", "Overlays");
            _fileListProvider = new FileListProvider(Directory, "overlay");
        }

        public List<string> GetEntries()
        {
            var entries = new List<string>();
            var files = _fileListProvider.GetFiles();

            foreach (var file in files) {
                entries.Add(Path.GetFileNameWithoutExtension(file));
            }

            return entries;
        }

        public string? LoadOSDFileContent(string fileName)
        {
            var path = Path.Combine(Directory, fileName + ".overlay");

            var content = _fileLoader.LoadFile(path);

            if (content == null) {
                return null;
            }

            var filtered = content?.Replace("\n", "");
            filtered = filtered?.Replace("\r", "");
            filtered = filtered?.Replace("<BR>", "\n");

            return filtered;
        }

        public void SaveOSDFileContent(string fileName, string content)
        {
            try
            {
                var path = Path.Combine(Directory, fileName + ".overlay");
                _fileSaver.SaveStringToFile(path, content);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public FileSystemWatcher Watch(Action action)
        {
            var watcher = new FileSystemWatcher();
            watcher.Path = Directory;
            watcher.Filter = "*.overlay";
            watcher.NotifyFilter = NotifyFilters.Attributes |
                NotifyFilters.CreationTime |
                NotifyFilters.FileName |
                NotifyFilters.LastAccess |
                NotifyFilters.LastWrite |
                NotifyFilters.Size |
                NotifyFilters.Security;
            watcher.Created += (s, e) =>
            {
                action();
                Log.Info($"Overlay created {e.Name}.");
            };
            watcher.Deleted += (s, e) =>
            {
                Log.Info($"Overlay deleted {e.Name}.");
                action();
            };
            watcher.Renamed += (s, e) =>
            {
                Log.Info($"Overlay renamed {e.OldName}.");
                action();
            };

            watcher.EnableRaisingEvents = true;

            return watcher;
        }
    }
}
