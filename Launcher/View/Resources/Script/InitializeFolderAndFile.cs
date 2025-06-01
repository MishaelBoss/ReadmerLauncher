using LauncherLes1.View.Resources.Script;
using System.IO;

namespace Launcher.View.Resources.Script
{
    public class InitializeFolderAndFile
    {
        public static void Initialize() {
            if (!Directory.Exists(Paths.config)) Directory.CreateDirectory(Paths.config);
            if (!Directory.Exists(Paths.work)) Directory.CreateDirectory(Paths.work);
            if (!Directory.Exists(Paths.download)) Directory.CreateDirectory(Paths.download);
            if (!Directory.Exists(Paths.common)) Directory.CreateDirectory(Paths.common);
            if (!Directory.Exists(Paths.saved)) Directory.CreateDirectory(Paths.saved);
            if (!Directory.Exists(Paths.crashes)) Directory.CreateDirectory(Paths.crashes);
            if (!Directory.Exists(Paths.log)) Directory.CreateDirectory(Paths.log);
        }
    }
}
