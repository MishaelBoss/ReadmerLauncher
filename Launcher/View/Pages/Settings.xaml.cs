using CheckConnectInternet;
using Launcher.Model;
using Launcher.View.Resources.Script;
using Launcher.View.Windows;
using LauncherLes1.View.Resources.Script;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;

namespace Launcher.View.Pages
{
    public partial class Settings : Page
    {
        private DownloadUpdateLauncherWindow _updateLauncherWindow;
        private const string ConfigFileName = "appsettings.xml";

        public Settings()
        {
            InitializeComponent();
            InitializeSettings();
            InitializeVersionInfo();
        }

        private void InitializeSettings()
        {
            try
            {
                if (!Directory.Exists(Paths.config))
                {
                    Directory.CreateDirectory(Paths.config);
                }

                var configPath = Path.Combine(Paths.config, ConfigFileName);
                if (!File.Exists(configPath))
                {
                    XmlConfigCreate.Create();
                }
                else
                {
                    XmlConfigReader.Load();
                }

                CheckBox.IsChecked = Arguments.Update_if_is_update;
                Autoload.IsChecked = Arguments.Autoload;
                Receive_notifications.IsChecked = Arguments.Receive_notifications;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации настроек: {ex.Message}", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InitializeVersionInfo()
        {
            currentVersion.Content = $"Моя версия: {Arguments.curverVersion}";

            if (Internet.connect()) _ = CheckForUpdatesAsync();

        }

        private async Task CheckForUpdatesAsync()
        {
            try
            {
                await CheckUpdateJson();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка при проверке обновлений: {ex.Message}");
                Dispatcher.Invoke(() =>
                    MessageBox.Show("Не удалось проверить обновления", "Ошибка",
                                 MessageBoxButton.OK, MessageBoxImage.Warning));
            }
        }

        private async Task CheckUpdateJson()
        {
            using var client = new HttpClient();
            var json = await client.GetStringAsync(Arguments.urlJSONUpdateLauncher);
            var data = JsonConvert.DeserializeObject<UpdateLauncher>(json);

            Arguments.fileDownloadLink = data.fileDownloadLink;
            Arguments.newVersion = data.version;
            Arguments.aboutVersionLink = data.aboutVersionLink;
            Arguments.readver = data.version;
            Arguments.isEmergencyUpdate = data.isEmergencyUpdate;

            Dispatcher.Invoke(() => UpdateUI(data));
        }

        private void UpdateUI(UpdateLauncher updateData)
        {
            var hasUpdate = Arguments.curverVersion.CompareTo(Arguments.readver) < 0;

            ButtonDownloadUpdate.Visibility = hasUpdate ? Visibility.Visible : Visibility.Hidden;
            ButtonCheckUpdate.Visibility = hasUpdate ? Visibility.Hidden : Visibility.Visible;
            ButtonInformationNewVersion.Visibility =
                !string.IsNullOrEmpty(updateData.aboutVersionLink) ? Visibility.Visible : Visibility.Hidden;

            currentVersion.Content = hasUpdate
                ? $"Моя версия: {Arguments.curverVersion}\nНовая версия: {Arguments.readver}"
                : $"Моя версия: {Arguments.curverVersion}";

            if (updateData.isEmergencyUpdate && _updateLauncherWindow == null)
            {
                ShowUpdateWindow();
            }
        }

        private void ShowUpdateWindow()
        {
            _updateLauncherWindow = new DownloadUpdateLauncherWindow();
            _updateLauncherWindow.Closed += (s, e) => _updateLauncherWindow = null;
            _updateLauncherWindow.Show();
        }

        private void CheckBoxChecked(object sender, RoutedEventArgs e)
        {
            var isChecked = CheckBox.IsChecked ?? false;
            XmlConfigSave.Change(0, "update_if_is_update", isChecked);
            Arguments.Update_if_is_update = isChecked;
        }

        private void AutoLoadingClick(object sender, RoutedEventArgs e)
        {
            var isChecked = Autoload.IsChecked ?? false;
            XmlConfigSave.Change(2, "autoload", isChecked);
            Arguments.Autoload = isChecked;
        }

        private void ReceiveNotificationsClick(object sender, RoutedEventArgs e)
        {
            var isChecked = Receive_notifications.IsChecked ?? false;
            XmlConfigSave.Change(1, "receive_notifications", isChecked);
            Arguments.Receive_notifications = isChecked;
        }

        private void ButtonDownloadUpdateClick(object sender, RoutedEventArgs e)
        {
            if (_updateLauncherWindow == null)
            {
                ShowUpdateWindow();
            }
        }

        private void ButtonCheckUpdateClick(object sender, RoutedEventArgs e)
        {
            _ = CheckForUpdatesAsync();
        }

        private void ButtonMoreInformationClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Arguments.aboutVersionLink))
                {
                    Process.Start(new ProcessStartInfo(Arguments.aboutVersionLink)
                    {
                        UseShellExecute = true
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}