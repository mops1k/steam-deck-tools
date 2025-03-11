namespace CommonHelpers.OSDService
{
    public class OSDFileManager
    {
        private string Directory { get; }
        private Dictionary<string, string> Entries { get; }
        private readonly FileListProvider _fileListProvider;
        private readonly FileSaver _fileSaver = new FileSaver();
        private readonly FileLoader _fileLoader = new FileLoader();

        public OSDFileManager()
        {
            var dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            Directory = Path.Combine(dir, "SteamDeckTools", "Overlays");
            _fileListProvider = new FileListProvider(Directory, "overlay");
            Entries = new Dictionary<string, string>();
        }

        public Dictionary<string, string> GetEntries()
        {
            if (Entries.Count > 0) {
                return Entries;
            }
            
            var files = _fileListProvider.GetFiles();

            foreach (var file in files) {
                Entries.Add(Path.GetFileNameWithoutExtension(file), file);
            }

            return Entries;
        }

        public string? LoadOSDFileContent(string fileName)
        {
            var path = Entries.GetValueOrDefault(fileName, String.Empty);
            if (path == String.Empty)
            {
                return null;
            }
            
            return _fileLoader.LoadFile(path);
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
    }
}
