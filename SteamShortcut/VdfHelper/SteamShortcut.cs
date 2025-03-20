using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Text;
using Force.Crc32;

namespace SteamShortcut.VdfHelper
{
    public static class GetSteamShortcutPath
    {
        private static int? _cachedUserId = null; // Кэшируем выбранный ID пользователя

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
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".steam", "steam", "userdata");
            }
        }

        public static int GetCurrentlyLoggedInUser()
        {
            // Если ID пользователя уже кэширован, возвращаем его
            if (_cachedUserId.HasValue)
                return _cachedUserId.Value;

            string userDataPath = GetUserDataPath();
            if (!Directory.Exists(userDataPath))
                return -1;

            List<DirectoryInfo> directories = new DirectoryInfo(userDataPath).GetDirectories().ToList();
            if (directories.Count == 0)
                return -1;

            // Если Steam не запущен, предлагаем пользователю выбрать юзера
            if (!IsSteamRunning())
            {
                // Если пользователь только один, выбираем его автоматически
                if (directories.Count == 1 && int.TryParse(directories[0].Name, out int singleUserId))
                {
                 _cachedUserId = singleUserId; // Кэшируем ID единственного пользователя
                 return _cachedUserId.Value;
                }

                _cachedUserId = PromptUserToSelectSteamUser(directories); // Кэшируем выбранный ID
                return _cachedUserId.Value;
            }

            // Если Steam запущен, возвращаем активного пользователя
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                _cachedUserId = (int)Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Valve\\Steam\\ActiveProcess", "ActiveUser", -1);
                return _cachedUserId.Value;
            }
            else
            {
                int a = -1;
                List<DirectoryInfo> validDirectories = directories.Where(x => int.TryParse(x.Name, out a) && File.Exists(Path.Join(x.FullName, "config", "localconfig.vdf"))).ToList();
                _cachedUserId = int.Parse(validDirectories.OrderByDescending(x => File.GetLastWriteTime(Path.Join(x.FullName, "config", "localconfig.vdf"))).First().Name);
                return _cachedUserId.Value;
            }
        }

        private static bool IsSteamRunning()
        {
            // Проверяем, запущен ли Steam (например, через поиск процесса)
            return System.Diagnostics.Process.GetProcessesByName("steam").Length > 0;
        }

        private static int PromptUserToSelectSteamUser(List<DirectoryInfo> directories)
        {
            // Создаем список пользователей с их именами
            var users = new List<(string Id, string Name)>();
            foreach (var dir in directories)
            {
                string localConfigPath = Path.Combine(dir.FullName, "config", "localconfig.vdf");
                if (File.Exists(localConfigPath))
                {
                    string username = GetUsernameFromLocalConfig(localConfigPath);
                    users.Add((dir.Name, username ?? "Unknown User"));
                }
                else
                {
                    users.Add((dir.Name, "Unknown User"));
                }
            }

            // Создаем форму для выбора пользователя
            using (var form = new Form())
            using (var label = new Label())
            using (var comboBox = new ComboBox())
            using (var buttonOk = new Button())
            {
                form.Text = "Выберите Steam-пользователя";
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.StartPosition = FormStartPosition.CenterScreen;
                form.MinimizeBox = false;
                form.MaximizeBox = false;

                // Настройка Label
                label.Text = "Пользователь:";
                label.AutoSize = true;
                label.Location = new System.Drawing.Point(10, 10); // Отступ слева и сверху

                // Настройка ComboBox
                comboBox.DataSource = users.Select(u => $"{u.Name} (ID: {u.Id})").ToList();
                comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                comboBox.Location = new System.Drawing.Point(10, label.Bottom + 5); // Располагаем под Label
                comboBox.Width = 250; // Фиксированная ширина

                // Настройка кнопки OK
                buttonOk.Text = "OK";
                buttonOk.DialogResult = DialogResult.OK;
                buttonOk.Location = new System.Drawing.Point(comboBox.Right - buttonOk.Width, comboBox.Bottom + 10); // Располагаем под ComboBox

                // Настройка размеров формы
                form.ClientSize = new System.Drawing.Size(
                    Math.Max(comboBox.Right, buttonOk.Right) + 20, // Ширина формы
                    buttonOk.Bottom + 20 // Высота формы
                );

                // Добавляем элементы на форму
                form.Controls.Add(label);
                form.Controls.Add(comboBox);
                form.Controls.Add(buttonOk);

                // Отображаем форму и возвращаем результат
                if (form.ShowDialog() == DialogResult.OK)
                {
                    // Получаем выбранный ID пользователя
                    string selectedUser = comboBox.SelectedItem.ToString();
                    string selectedId = selectedUser.Split(new[] { "(ID: " }, StringSplitOptions.None)[1].TrimEnd(')');
                    if (int.TryParse(selectedId, out int selectedUserId))
                    {
                        return selectedUserId;
                    }
                }
            }

            return -1; // Если пользователь не выбрал ничего
        }

        private static string GetUsernameFromLocalConfig(string localConfigPath)
        {
            try
            {
                // Читаем файл localconfig.vdf
                var lines = File.ReadAllLines(localConfigPath);
                foreach (var line in lines)
                {
                    // Ищем строку с именем пользователя
                    if (line.Contains("PersonaName"))
                    {
                        // Пример строки: "PersonaName"		"Username"
                        var parts = line.Split(new[] { '\t', '"' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length >= 2)
                        {
                            return parts[1];
                        }
                    }
                }
            }
            catch
            {
                // В случае ошибки возвращаем null
                return null;
            }

            return null;
        }

        public static string GetShortcutsPath()
        {
            int userId = GetCurrentlyLoggedInUser();
            if (userId == -1)
                return "";

            return Path.Combine(GetUserDataPath(), userId.ToString(), "config", "shortcuts.vdf");
        }

        public static string GetGridPath()
        {
            int userId = GetCurrentlyLoggedInUser();
            if (userId == -1)
                return "";

            string path = Path.Combine(GetUserDataPath(), userId.ToString(), "config", "grid");
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
