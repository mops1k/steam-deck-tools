﻿namespace CommonHelpers.OSDService
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
