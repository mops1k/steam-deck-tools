using CommonHelpers;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Launcher.Helper
{
    public class ShortcutGenerator
    {
        public void GenerateShortcuts()
        {
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            var exePath = Process.GetCurrentProcess().MainModule?.FileName;

            if (String.IsNullOrEmpty(exePath))
            {
                Log.Error("Could not determine the current executable path.");
                return;
            }

            // Создаем ярлык для запуска всех инструментов
            CreateShortcut(Path.Combine(desktopPath, "Run SteamDeck Tools.lnk"), exePath, "--start");

            // Создаем ярлык для остановки всех инструментов
            CreateShortcut(Path.Combine(desktopPath, "Stop SteamDeck Tools.lnk"), exePath, "--stop");

            // Создаем ярлык для перезапуска всех инструментов
            CreateShortcut(Path.Combine(desktopPath, "Restart SteamDeck Tools.lnk"), exePath, "--restart");

            Log.Info("Shortcuts generated successfully.");
        }

        private void CreateShortcut(string shortcutPath, string targetPath, string arguments)
        {
            try
            {
                // Создаем объект IShellLink
                var shellLink = (IShellLink)new ShellLinkObject();

                // Устанавливаем свойства ярлыка
                shellLink.SetPath(targetPath);
                shellLink.SetArguments(arguments);
                shellLink.SetWorkingDirectory(Path.GetDirectoryName(targetPath));
                shellLink.SetDescription("SteamDeck Tools Launcher");
                shellLink.SetIconLocation(targetPath, 0);

                // Сохраняем ярлык
                var persistFile = (IPersistFile)shellLink;
                persistFile.Save(shortcutPath, false);
            }
            catch (Exception ex)
            {
                Log.Fatal("Failed to create shortcut.", ex);
            }
        }

        // Интерфейс IShellLink
        [ComImport]
        [Guid("000214F9-0000-0000-C000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IShellLink
        {
            void GetPath([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, int cchMaxPath, out IntPtr pfd, int fFlags);
            void GetIDList(out IntPtr ppidl);
            void SetIDList(IntPtr pidl);
            void GetDescription([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszName, int cchMaxName);
            void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);
            void GetWorkingDirectory([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir, int cchMaxPath);
            void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);
            void GetArguments([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs, int cchMaxPath);
            void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);
            void GetHotkey(out short pwHotkey);
            void SetHotkey(short wHotkey);
            void GetShowCmd(out int piShowCmd);
            void SetShowCmd(int iShowCmd);
            void GetIconLocation([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszIconPath, int cchIconPath, out int piIcon);
            void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);
            void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, int dwReserved);
            void Resolve(IntPtr hwnd, int fFlags);
            void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
        }

        // Интерфейс IPersistFile
        [ComImport]
        [Guid("0000010B-0000-0000-C000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IPersistFile
        {
            void GetClassID(out Guid pClassID);
            void IsDirty();
            void Load([MarshalAs(UnmanagedType.LPWStr)] string pszFileName, int dwMode);
            void Save([MarshalAs(UnmanagedType.LPWStr)] string pszFileName, [MarshalAs(UnmanagedType.Bool)] bool fRemember);
            void SaveCompleted([MarshalAs(UnmanagedType.LPWStr)] string pszFileName);
            void GetCurFile([MarshalAs(UnmanagedType.LPWStr)] out string ppszFileName);
        }

        // Класс ShellLinkObject
        [ComImport]
        [Guid("00021401-0000-0000-C000-000000000046")]
        private class ShellLinkObject
        {
        }
    }
}
