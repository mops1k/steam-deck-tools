namespace CommonHelpers.OSDService
{
    public class FileLoader
    {
        public string? LoadFile(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            return File.ReadAllText(path);
        }
    }
}
