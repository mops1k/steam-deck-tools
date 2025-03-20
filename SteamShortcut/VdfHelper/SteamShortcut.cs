using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Text;
using Force.Crc32;

namespace SteamShortcut.VdfHelper
{
    public static class GetSteamShortcutPath
    {
        public static string GetUserDataPath()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                string path = (string)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\Valve\\Steam", "InstallPath", null);
                if (path == null)
                    return "";
                return Path.Combine(path, "userdata");
            }
            else
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".steam","steam","userdata");
            }
        }

        public static int GetCurrentlyLoggedInUser()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return (int)Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Valve\\Steam\\ActiveProcess", "ActiveUser", -1);
            }
            else
            {
                int a = -1;

                List<DirectoryInfo> directories = new DirectoryInfo(GetUserDataPath()).GetDirectories().ToList();
                List<DirectoryInfo> validDirectories = directories.Where(x => int.TryParse(x.Name, out a) && File.Exists(Path.Join(x.FullName, "config", "localconfig.vdf"))).ToList();
                return int.Parse(validDirectories.OrderByDescending(x => File.GetLastWriteTime(Path.Join(x.FullName, "config", "localconfig.vdf"))).First().Name);
            }
        }

        public static string GetShortcutsPath()
        {
            return Path.Combine(GetUserDataPath(), GetCurrentlyLoggedInUser().ToString(), "config", "shortcuts.vdf");
        }

        public static string GetGridPath()
        {
            string path = Path.Combine(GetUserDataPath(), GetCurrentlyLoggedInUser().ToString(), "config", "grid");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }
    }
    
    public class ShortcutEntry
    {
        public VDFMap Raw { get; private set; }

        public uint AppId { get => ReadInt("appid"); set => WriteInt("appid", value); }
        public string AppName { get => ReadString("appName"); set => WriteString("appName", value); }
        public string Exe { get => ReadString("exe"); set => WriteString("exe", value); }
        public string StartDir { get => ReadString("StartDir"); set => WriteString("StartDir", value); }
        public string Icon { get => ReadString("icon"); set => WriteString("icon", value); }
        public string ShortcutPath { get => ReadString("ShortcutPath"); set => WriteString("ShortcutPath", value); }
        public string LaunchOptions { get => ReadString("LaunchOptions"); set => WriteString("LaunchOptions", value); }
        public uint IsHidden { get => ReadInt("IsHidden"); set => WriteInt("IsHidden", value); }
        public uint AllowDesktopConfig { get => ReadInt("AllowDesktopConfig"); set => WriteInt("AllowDesktopConfig", value); }
        public uint AllowOverlay { get => ReadInt("AllowOverlay"); set => WriteInt("AllowOverlay", value); }
        public uint Openvr { get => ReadInt("openvr"); set => WriteInt("openvr", value); }
        public uint Devkit { get => ReadInt("Devkit"); set => WriteInt("Devkit", value); }
        public string DevkitGameID { get => ReadString("DevkitGameID"); set => WriteString("DevkitGameID", value); }
        public uint DevkitOverrideAppID { get => ReadInt("DevkitOverrideAppID"); set => WriteInt("DevkitOverrideAppID", value); }
        public uint LastPlayTime { get => ReadInt("LastPlayTime"); set => WriteInt("LastPlayTime", value); }

        public ShortcutEntry(VDFMap raw)
        {
            Raw = raw;

            if (raw == null)
                throw new Exception("Shortcut entry is null!");
        }

        public int GetTagsSize() => Raw.GetValue("tags").ToMap().GetSize();
        public string GetTag(int idx) => Raw.GetValue("tags").ToMap().GetValue(idx.ToString()).Text;
        public void SetTag(int idx, string value) => Raw.GetValue("tags").ToMap().GetValue(idx.ToString()).Text = value;
        public void AddTag(string value) => Raw.GetValue("tags").ToMap().Map.Add(GetTagsSize().ToString(), new VDFString(value));
        public void RemoveTag(int idx) => Raw.GetValue("tags").ToMap().RemoveFromArray(idx);
        public int GetTagIndex(string value)
        {
            for (int i = 0; i < GetTagsSize(); i++)
            {
                if (GetTag(i) == value)
                    return i;
            }

            return -1;
        }

        private uint ReadInt(string key) => Raw.GetValue(key).Integer;
        private void WriteInt(string key, uint value) => Raw.GetValue(key).Integer = value;

        private string ReadString(string key) => Raw.GetValue(key)?.Text ?? null;
        private void WriteString(string key, string value) => Raw.GetValue(key).Text = value;

        public static uint GenerateSteamGridAppId(string appName, string appTarget)
        {
            byte[] nameTargetBytes = Encoding.UTF8.GetBytes(appTarget + appName + "");
            uint crc = Crc32Algorithm.Compute(nameTargetBytes);
            uint gameId = crc | 0x80000000;

            return gameId;
        }
    }
    
    public class ShortcutRoot(VDFMap root)
    {
        public VDFMap root { get; private set; } = root;
        private VDFMap GetShortcutMap() => root.GetValue("shortcuts").ToMap();

        public int GetSize() => GetShortcutMap().GetSize();

        public ShortcutEntry GetEntry(int entry) => new ShortcutEntry(GetShortcutMap().ToMap().GetValue(entry.ToString()).ToMap());

        public ShortcutEntry AddEntry()
        {
            VDFMap entry = new VDFMap();
            entry.FillWithDefaultShortcutEntry();

            GetShortcutMap().Map.Add(GetSize().ToString(), entry);
            return new ShortcutEntry(entry);
        }

        public void RemoveEntry(int idx)
        {
            GetShortcutMap().RemoveFromArray(idx);
        }
    }
}
