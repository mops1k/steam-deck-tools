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

        public void Watch(Action action)
        {
            var watcher = new FileSystemWatcher();
            watcher.Path = Directory;
            watcher.Filter = "*.overlay";
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Created += (s, e) => action();
            watcher.Deleted += (s, e) => action();
            
            watcher.EnableRaisingEvents = true;
        }
    }
}
