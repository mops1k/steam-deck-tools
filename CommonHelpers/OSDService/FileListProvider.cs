namespace CommonHelpers.OSDService
{
    public class FileListProvider
    {
        private string? Directory { get; }
        private string Extension { get; } = "txt";
        
        public FileListProvider(string directory)
        {
            if (!System.IO.Directory.Exists(directory)) {
                return;
            }
            
            Directory = directory;
        }
        
        public FileListProvider(string directory, string? extension)
        {
            if (!System.IO.Directory.Exists(directory)) {
                return;
            }
            
            Directory = directory;
            if (extension == null) {
                return;
            }
            
            Extension = extension;
        }

        public List<string> GetFiles()
        {
            if (Directory == null) {
                return new List<string>();
            }
            
            return System.IO.Directory
                .EnumerateFiles(Directory, "*."+Extension, SearchOption.AllDirectories)
                .ToList()
            ;
        }
    }
}
