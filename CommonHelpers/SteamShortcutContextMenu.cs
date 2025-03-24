using System.Diagnostics;
using System.Reflection;
namespace CommonHelpers
{
    public static class SteamShortcutContextMenu
    {
        private static string MenuName = "Add to Steam";
        private static string ExeFullPath
        {
            get
            {
                var exePath = Assembly.GetEntryAssembly()?.Location;
                if (exePath == null)
                {
                    return null;
                }

                return Path.Combine(Path.GetDirectoryName(exePath), "SteamShortcut.exe");
            }
        }

        public static bool IsExists()
        {
            return ContextMenuManager.IsContextMenuExists("exefile", MenuName);
        }

        public static void Add()
        {
            if (ExeFullPath == null)
            {
                throw new FileNotFoundException("Could not find the SteamShortcut executable.");
            }

            if (!File.Exists(ExeFullPath))
            {
                throw new FileNotFoundException($"SteamShortcut executable not found at: {ExeFullPath}");
            }

            ContextMenuManager.AddContextMenu("exefile", MenuName, $"{ExeFullPath} \"%1\"", $"\"{ExeFullPath}\",0");
        }

        public static void Remove()
        {
            ContextMenuManager.RemoveContextMenu("exefile", MenuName);
        }
    }
}
