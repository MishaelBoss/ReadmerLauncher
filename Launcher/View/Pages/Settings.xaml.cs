using CheckConnectInternet;
using Launcher.Model;
using Launcher.View.Windows;
using Newtonsoft.Json;
using System.Net.Http;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace Launcher.View.Pages
{
    public partial class Settings : Page
    {
        private readonly string urlJSON = "https://raw.githubusercontent.com/RedmerGameAndTechnologies/JsonLauncher/refs/heads/main/VersionLauncher.json";

        private string curverVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public static string fileDownload;

        public Settings()
        {
            InitializeComponent();
            CheckUpdate();
        }

        private async Task CheckUpdateJson(string urlJSON)
        {
            using HttpClient client = new HttpClient();
            string json = await client.GetStringAsync(urlJSON);
            UpdateLauncher data = JsonConvert.DeserializeObject<UpdateLauncher>(json);

            string fileDownloadLink = data.fileDownloadLink;
            string newVersion = data.version;
            fileDownload = fileDownloadLink;

            string readver = newVersion;

            readver = readver.Replace(".", ".");
            curverVersion = curverVersion.Replace(".", ".");

            if (curverVersion.CompareTo(readver) < 0)
            {
                ButtonDownloadUpdate.Visibility = Visibility.Visible;
                ButtonCheckUpdate.Visibility = Visibility.Hidden;
                currentVersion.Content = "Моя версия: " + curverVersion + " \nНовая версия: " + readver;
            }
            else
            {
                ButtonDownloadUpdate.Visibility = Visibility.Hidden;
                ButtonCheckUpdate.Visibility = Visibility.Visible;
                currentVersion.Content = "Моя версия: " + curverVersion;
            }
        }

        private void CheckUpdate() {
            if (!Internet.connect())
                return;

            Task task = CheckUpdateJson(urlJSON);
        }


        private void CheckBox_Cecked(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonDownloadUpdateClick(object sender, RoutedEventArgs e)
        {
            DownloadUpdateLauncherWindow downloadUpdateLauncherWindow = new DownloadUpdateLauncherWindow();
            downloadUpdateLauncherWindow.Show();
        }

        private void ButtonCheckUpdateClick(object sender, RoutedEventArgs e)
        {
            CheckUpdate();
        }

        private void ButtonMoreInformationClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
