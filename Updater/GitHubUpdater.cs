using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Updater
{
    public class GitHubUpdater
    {
        private readonly string _repoOwner;
        private readonly string _repoName;
        private readonly string _currentVersion;
        private readonly string _downloadPath;

        public GitHubUpdater(string repoOwner, string repoName, string currentVersion, string downloadPath = "updates")
        {
            _repoOwner = repoOwner;
            _repoName = repoName;
            _currentVersion = currentVersion;
            _downloadPath = downloadPath;
        }

        public async Task CheckAndUpdateAsync()
        {
            try
            {
                var latestRelease = await GetLatestReleaseAsync();
                var latestVersion = latestRelease["tag_name"]?.ToString();

                if (IsNewVersionAvailable(latestVersion))
                {
                    Console.WriteLine($"New version {latestVersion} is available!");
                    var downloadUrl = latestRelease["assets"][0]["browser_download_url"].ToString();
                    await DownloadAndInstallUpdateAsync(downloadUrl);
                }
                else
                {
                    Console.WriteLine("You are using the latest version.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during update check: {ex.Message}");
            }
        }

        private async Task<JObject> GetLatestReleaseAsync()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "GitHubUpdater");
                var response = await client.GetStringAsync($"https://api.github.com/repos/{_repoOwner}/{_repoName}/releases/latest");
                return JObject.Parse(response);
            }
        }

        private bool IsNewVersionAvailable(string? latestVersion)
        {
            if (latestVersion == null)
            {
                return false;
            }
            
            var currentVersion = new Version(_currentVersion);
            var latest = new Version(latestVersion);
            
            return latest > currentVersion;
        }

        private async Task DownloadAndInstallUpdateAsync(string downloadUrl)
        {
            if (!Directory.Exists(_downloadPath))
            {
                Directory.CreateDirectory(_downloadPath);
            }

            var fileName = Path.GetFileName(new Uri(downloadUrl).LocalPath);
            var filePath = Path.Combine(_downloadPath, fileName);

            using (var client = new HttpClient())
            {
                var fileBytes = await client.GetByteArrayAsync(downloadUrl);
                await File.WriteAllBytesAsync(filePath, fileBytes);
            }

            Console.WriteLine($"Downloaded update to {filePath}");
            InstallUpdate(filePath);
        }

        private void InstallUpdate(string filePath)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = filePath,
                UseShellExecute = true
            };

            Process.Start(processStartInfo);
            Environment.Exit(0);
        }
    }
}
