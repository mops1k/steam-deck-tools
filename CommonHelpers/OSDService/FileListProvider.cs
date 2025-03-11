using Filesystem = System.IO.Directory;
namespace CommonHelpers.OSDService
{
    public class FileListProvider
    {
        private string Directory { get; }
        private string Extension { get; } = "txt";
        
        public FileListProvider(string directory)
        {
            if (!Filesystem.Exists(directory))
            {
                Filesystem.CreateDirectory(directory);
            }
            
            Directory = directory;
        }
        
        public FileListProvider(string directory, string? extension)
        {
            if (!Filesystem.Exists(directory))
            {
                Filesystem.CreateDirectory(directory);
            }
            
            Directory = directory;
            if (extension == null) {
                return;
            }
            
            Extension = extension;
        }

        public List<string> GetFiles()
        {
            return Filesystem.EnumerateFiles(Directory, "*."+Extension, SearchOption.AllDirectories)
                .ToList()
            ;
        }
    }
}
