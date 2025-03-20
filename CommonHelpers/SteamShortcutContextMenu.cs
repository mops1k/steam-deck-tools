﻿using System.Diagnostics;
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
                
                return Path.Combine(exePath, "SteamShortcut.exe");
            }
        }

        public static bool IsExists()
        {
            return ContextMenuManager.IsContextMenuExists(".exe", MenuName);
        }

        public static void Add()
        {
            if (ExeFullPath == null)
            {
                throw new FileNotFoundException("Could not find the SteamShortcut executable.");
            }
            
            ContextMenuManager.AddContextMenu(".exe", MenuName, ExeFullPath + " \"%1\"");
        }

        public static void Remove()
        {
            ContextMenuManager.RemoveContextMenu(".exe", MenuName);
        }
    }
}
