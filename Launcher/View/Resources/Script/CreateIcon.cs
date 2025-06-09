using IWshRuntimeLibrary;
using System.IO;

namespace Launcher.View.Resources.Script
{
    class CreateIcon
    {
        public static void CreateShortcut(ShortcutLocation location, string name)
        {
            try
            {
                if (location == ShortcutLocation.All)
                {
                    CreateShortcut(ShortcutLocation.DESKTOP, name);
                    CreateShortcut(ShortcutLocation.START_MENU, name);
                    return;
                }

                object shortPath;

                switch (location)
                {
                    case ShortcutLocation.DESKTOP:
                        shortPath = "Desktop";
                        break;
                    case ShortcutLocation.START_MENU:
                        shortPath = "StartMenu";
                        break;
                    default:
                        throw new ArgumentException("Invalid location");
                }

                WshShell shell = new WshShell();
                string shortcutAddress = (string)shell.SpecialFolders.Item(ref shortPath) + $@"\{name}.lnk";

                string iconPath = Path.GetFullPath(Path.Combine(Paths.games, $"{name}.ico"));

                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
                shortcut.IconLocation = iconPath;
                //shortcut.TargetPath = $"{Paths.common}/{name}";
                shortcut.TargetPath = Arguments.execPath;
                shortcut.Save();
            }
            catch (Exception ex)
            {
                Loges.LoggingProcess(LogLevel.ERROR, ex: ex);
            }
        }
    }

    public enum ShortcutLocation
    {
        DESKTOP,
        START_MENU,
        All
    }
}