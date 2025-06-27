using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Windows;

namespace Launcher.View.Resources.Script.Json
{
    public class JsonConfidCreate
    {
        public static void CreateLibraryFolders(string path) {
            var defaultData = new
            {
                apps = new JObject()
            };

            if(File.Exists(Paths.work)) File.WriteAllText(path, JsonConvert.SerializeObject(defaultData, Formatting.Indented));
            else Directory.CreateDirectory(Paths.work);
        }

        public static void Create(string nameGame, string gamePath) {
            string LibraryfoldersFilePath = Path.Combine(Paths.work, Files.LibraryfoldersJson);
            string manifestFileName = $"appmanifest_{nameGame}.json";
            string manifestPath = Path.Combine(Paths.work, manifestFileName);

            try
            {
                var manifestData = new
                {
                    name = nameGame,
                    install_dir = gamePath
                };
                File.WriteAllText(manifestPath, JsonConvert.SerializeObject(manifestData, Formatting.Indented));

                var libraryData = JObject.Parse(File.ReadAllText(LibraryfoldersFilePath));
                libraryData["apps"][nameGame] = manifestPath;
                File.WriteAllText(LibraryfoldersFilePath, libraryData.ToString(Formatting.Indented));

                MessageBox.Show($"Игра {nameGame} добавлена в библиотеку!\nФайл: {manifestPath}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}
