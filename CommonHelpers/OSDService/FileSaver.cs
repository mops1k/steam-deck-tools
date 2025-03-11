namespace CommonHelpers.OSDService
{
    public class FileSaver
    {
        public void SaveStringToFile(string filePath, string content)
        {
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, content);
            }
        }
    }
}
