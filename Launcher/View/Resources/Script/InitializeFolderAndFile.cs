using Launcher.View.Resources.Script.Json;
using System.IO;

namespace Launcher.View.Resources.Script
{
    public class InitializeFolderAndFile
    {
        public static void Initialize() {
            string LibraryfoldersFilePath = Path.Combine(Paths.work, Files.LibraryfoldersJson);

            if (!Directory.Exists(Paths.config)) Directory.CreateDirectory(Paths.config);
            if (!Directory.Exists(Paths.work)) Directory.CreateDirectory(Paths.work);
            if (!Directory.Exists(Paths.download)) Directory.CreateDirectory(Paths.download);
            if (!Directory.Exists(Paths.common)) Directory.CreateDirectory(Paths.common);
            if (!Directory.Exists(Paths.userdata)) Directory.CreateDirectory(Paths.userdata);

            if (!File.Exists(LibraryfoldersFilePath)) ManagerLibraryJson.CreateLibraryFolders();
        }
    }
}
