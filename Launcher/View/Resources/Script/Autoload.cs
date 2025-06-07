using LauncherLes1.View.Resources.Script;
using Microsoft.Win32;

namespace Launcher.View.Resources.Script
{
    public static class Autoload
    {
        public static bool SetAutoload() {
            RegistryKey registryKey;
            registryKey = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\");
            try {
                if (Arguments.Autoload) registryKey.SetValue(Arguments.exenames, Arguments.execPath);
                else registryKey.DeleteValue(Arguments.exenames); registryKey.Flush();
                registryKey.Close();
            }
            catch (Exception ex){
                Loges.LoggingProcess(LogLevel.INFO, ex: ex);
                return false;
            }
            return false;
        }
    }
}
