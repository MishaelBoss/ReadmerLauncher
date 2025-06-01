using Launcher.View.Windows;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace LauncherLes1.View.Resources.Script
{
    class Arguments
    {
        public static readonly string urlJSONUpdateLauncher = "https://raw.githubusercontent.com/RedmerGameAndTechnologies/JsonLauncher/refs/heads/main/VersionLauncher.json";

        #region Confirm Update
        public static string execPath = Process.GetCurrentProcess().MainModule.FileName;
        public static string workingDir = Path.GetDirectoryName(execPath);
        public static string sourcePath = Path.Combine(workingDir, Path.GetFileName(execPath));
        public static string exenames = Path.GetFileName(sourcePath);
        #endregion

        #region Settings
        public static string curverVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        #endregion

        public static bool Update_if_is_update { get; set; }
        public static bool Autoload { get; set; }
        public static bool Receive_notifications { get; set; }


        public static string newVersion { get; set; }
        public static string fileDownloadLink { get; set; }
        public static string aboutVersion { get; set; }
        public static string aboutVersionLink { get; set; }
        public static string readver { get; set; }
        public static bool isEmergencyUpdate { get; set; }
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
        public static readonly string appTemlPath = "tempDirectoryUnzip";
        #endregion

        #region Confirm Update
        public static readonly string zipPathUpdate = @".\UpdateLaucnher.zip";
        public static readonly string exeLauncherUpdate = @".\NewLauncher.exe";
        public static readonly string exetraPath = @".\";
        #endregion

        #region Settings
        public static readonly string config = @".\config";
        #endregion

        #region WorkFolders
        public static readonly string work = @".\apps";
        public static readonly string download = @$".\{work}\downloading";
        public static readonly string common = @$".\{work}\common";
        public static readonly string saved = @$".\Saved";
        public static readonly string crashes = @$".\{saved}\crashes";
        public static readonly string log = @$".\{saved}\log";
        #endregion
    }
}
