using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Launcher.View.Resources.Script
{
    class Files {
        public const string configSettingsFileName = "appsettings.xml";
        public const string LibraryfoldersJson = "libraryfolders.json";
    }

    class Arguments
    {
        public const string urlJSONUpdateLauncher = "https://raw.githubusercontent.com/RedmerGameAndTechnologies/JsonLauncher/refs/heads/main/VersionLauncher.json";

        #region Confirm Update
        public static string execPath = Process.GetCurrentProcess().MainModule.FileName;
        public static string workingDir = Path.GetDirectoryName(execPath);
        public static string sourcePath = Path.Combine(workingDir, Path.GetFileName(execPath));
        public static string exenames = Path.GetFileName(sourcePath);
        #endregion

        #region Settings
        public static string curverVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        #endregion

        #region XmlConfig
        public static bool Update_if_is_update { get; set; }
        public static bool Autoload { get; set; }
        public static bool Receive_notifications { get; set; }
        public static int Speed_download_update { get; set; }
        public static int Speed_download_game { get; set; }
        #endregion

        #region JsonUpdate
        public static string newVersion { get; set; }
        public static string fileDownloadLink { get; set; }
        public static string aboutVersion { get; set; }
        public static string aboutVersionLink { get; set; }
        public static string readver { get; set; }
        public static bool isEmergencyUpdate { get; set; }
        #endregion

        #region DB
        private const string ip = "localhost";
        private const string port = "5432";
        private const string database = "readmer_launcher";
        private const string userId = "postgres";
        private const string password = "cr2032";

        public const string connection = $"Server={ip};Port={port};Database={database}; User id = {userId};Password={password};";
        #endregion
    }

    class Paths
    {
        public string Name { get; set; }

        #region Download Game
        public readonly string zipPath;
        public Paths(string name)
        {
            Name = name;
            zipPath = $@".\ChacheDownloadGame_{name}.zip";
        }
        public static string GetZipPath(string name)
        {
            return $@".\ChacheDownloadGame_{name}.zip";
        }
        public const string appTemlPath = "tempDirectoryUnzip";
        #endregion

        #region Confirm Update
        public const string zipPathUpdate = @".\UpdateLaucnher.zip";
        public const string exeLauncherUpdate = @".\NewLauncher.exe";
        public const string exetraPath = @".\";
        #endregion

        #region Settings
        public  const string config = @".\config";
        #endregion

        #region WorkFolders
        public const string work = @".\apps";
        public const string download = @$".\{work}\downloading";
        public const string common = @$".\{work}\common";
        public const string saved = @$".\Saved";
        public const string crashes = @$".\{saved}\crashes";
        public const string log = @$".\{saved}\log";
        public const string readmer = @".\readmer";
        public const string games = @$".\{readmer}\games";
        public const string appcache = @".\appcache";
        public const string librarycache = @$".\{appcache}\librarycache";
        public const string userdata = @".\userdata";
        public const string avatarcache = @$".\{config}\avatarcache";
        #endregion

        public static string cookie(string username) {
            return Path.Combine(userdata, username);
        }

        public static string appLibrarycache(string name)
        {
            return Path.Combine(librarycache, name);
        }

        public static string destinationPath(string directory, string name, string extension)
        {
            return Path.Combine(directory, $"{name}.{extension}");
        }
    }
}
