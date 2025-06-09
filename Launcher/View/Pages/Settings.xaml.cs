using Launcher.View.Resources.Script;
using Launcher.View.Windows;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Launcher.View.Pages
{
    public partial class Settings : Page
    {
        private DownloadUpdateLauncherWindow _updateLauncherWindow;

        public Settings()
        {
            InitializeComponent();
            InitializeSettings();
            InitializeVersionInfo();
            Update.UpdateUI(BackgroundUIFunction: BackgroundUIFunction, hours: 0, minutes: 0, seconds: 5);
        }

        private void BackgroundUIFunction(object sender, EventArgs ea)
        {
            if (Internet.connect()) _ = CheckForUpdatesAsync();
            else Loges.LoggingProcess(LogLevel.INFO, "Подключитесь к интернету");
        }

        private void InitializeSettings()
        {
            try
            {
                var configPath = Path.Combine(Paths.config, Files.configSettingsFileName);
                if (!File.Exists(configPath)) XmlConfigCreate.Create();
                else XmlConfigReader.Load();

                CheckBox.IsChecked = Arguments.Update_if_is_update;
                Autoload.IsChecked = Arguments.Autoload;
                Receive_notifications.IsChecked = Arguments.Receive_notifications;
            }
            catch (Exception ex)
            {
                Loges.LoggingProcess(LogLevel.WARN, ex: ex);
                MessageBox.Show($"Ошибка инициализации настроек: {ex.Message}", "Ошибка",MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InitializeVersionInfo()
        {
            currentVersion.Content = $"Моя версия: {Arguments.curverVersion}";

            if (Internet.connect()) _ = CheckForUpdatesAsync();
            else Loges.LoggingProcess(LogLevel.INFO, "Подключитесь к интернету");

        }

        private async Task CheckForUpdatesAsync()
        {
            try
            {
                await CheckUpdateJson();
            }
            catch (Exception ex)
            {
                Loges.LoggingProcess(LogLevel.ERROR, ex:ex);
                Dispatcher.Invoke(() => MessageBox.Show("Не удалось проверить обновления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning));
            }
        }

        private async Task CheckUpdateJson()
        {
            await CheckUpdate.CheckUpdateJson();

            Dispatcher.Invoke(() => UpdateUI());
        }

        private void UpdateUI()
        {
            var hasUpdate = Arguments.curverVersion.CompareTo(Arguments.readver) < 0;

            ButtonDownloadUpdate.Visibility = hasUpdate ? Visibility.Visible : Visibility.Hidden;
            ButtonCheckUpdate.Visibility = hasUpdate ? Visibility.Hidden : Visibility.Visible;
            ButtonInformationNewVersion.Visibility = !string.IsNullOrEmpty(Arguments.aboutVersionLink) ? Visibility.Visible : Visibility.Hidden;

            currentVersion.Content = hasUpdate
                ? $"Моя версия: {Arguments.curverVersion}\nНовая версия: {Arguments.readver}"
                : $"Моя версия: {Arguments.curverVersion}";

            if (Arguments.isEmergencyUpdate && _updateLauncherWindow == null)
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
            XmlConfigSave.Change("update_if_is_update", true);
            Arguments.Update_if_is_update = true;
        }


        private void CheckBoxUnchecked(object sender, RoutedEventArgs e)
        {
            XmlConfigSave.Change("update_if_is_update", false);
            Arguments.Update_if_is_update = false;
        }

        private void AutoLoadingClick(object sender, RoutedEventArgs e)
        {
            XmlConfigSave.Change("autoload", true);
            Arguments.Autoload = true;
        }

        private void AutoLoadingUnchecked(object sender, RoutedEventArgs e)
        {
            XmlConfigSave.Change("autoload", false);
            Arguments.Autoload = false;
        }

        private void ReceiveNotificationsClick(object sender, RoutedEventArgs e)
        {
            XmlConfigSave.Change("receive_notifications", true);
            Arguments.Receive_notifications = true;
        }

        private void ReceiveNotificationsUnchecked(object sender, RoutedEventArgs e)
        {
            XmlConfigSave.Change("receive_notifications", false);
            Arguments.Receive_notifications = false;
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

        private void TextBoxArgumentsSpeedDownloadPreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
/*            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
            XmlConfigSave.Change("speed/update", e.Text);*/
        }

        private void TextBoxArgumentsSpeedDownloadGamePreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
/*            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
            XmlConfigSave.Change("speed/game", e.Text);*/
        }

        private bool ComboBoxChooseSpeedDownloadUpdateHandle = true;
        private bool ComboBoxChooseSpeedDownloadGameHandle = true;

        private void ComboBoxChooseSpeedDownloadUpdateSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxChooseSpeedDownloadUpdateHandle)ComboBoxChooseSpeedDownloadUpdate_Handle();
            ComboBoxChooseSpeedDownloadUpdateHandle = true;
        }

        private void ComboBoxChooseSpeedDownloadUpdateDropDownClosed(object sender, EventArgs e)
        {
            ComboBox ComboBoxChooseGameInLauncher = sender as ComboBox;
            ComboBoxChooseSpeedDownloadUpdateHandle = !ComboBoxChooseGameInLauncher.IsDropDownOpen;
            ComboBoxChooseSpeedDownloadUpdate_Handle();
        }

        private void ComboBoxChooseSpeedDownloadUpdate_Handle()
        {
            switch (ComboBoxChooseSpeedDownloadUpdate.SelectedIndex)
            {
                case 1:
                    XmlConfigSave.Change("speed/update", 999999);
                    break;
                case 2:
                    XmlConfigSave.Change("speed/update", 128000);
                    break;
                case 3:
                    XmlConfigSave.Change("speed/update", 256000);
                    break;
                case 4:
                    XmlConfigSave.Change("speed/update", 512000);
                    break;
                case 5:
                    XmlConfigSave.Change("speed/update", 1000000);
                    break;
                case 6:
                    XmlConfigSave.Change("speed/update", 2000000);
                    break;
                case 7:
                    XmlConfigSave.Change("speed/update", 3000000);
                    break;
                case 8:
                    XmlConfigSave.Change("speed/update", 5000000);
                    break;
                case 9:
                    XmlConfigSave.Change("speed/update", 10000000);
                    break;
                case 10:
                    XmlConfigSave.Change("speed/update", 25000000);
                    break;
            }
        }

        private void ComboBoxChooseSpeedDownloadGameSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxChooseSpeedDownloadGameHandle) ComboBoxChooseSpeedDownloadGame_Handle();
            ComboBoxChooseSpeedDownloadGameHandle = true;
        }

        private void ComboBoxChooseSpeedDownloadGameDropDownClosed(object sender, EventArgs e)
        {
            ComboBox ComboBoxChooseSpeedDownloadGame = sender as ComboBox;
            ComboBoxChooseSpeedDownloadGameHandle = !ComboBoxChooseSpeedDownloadGame.IsDropDownOpen;
            ComboBoxChooseSpeedDownloadGame_Handle();
        }

        private void ComboBoxChooseSpeedDownloadGame_Handle()
        {
            switch (ComboBoxChooseSpeedDownloadGame.SelectedIndex)
            {
                case 1:
                    XmlConfigSave.Change("speed/game", 999999);
                    break;
                case 2:
                    XmlConfigSave.Change("speed/game", 128000);
                    break;
                case 3:
                    XmlConfigSave.Change("speed/game", 256000);
                    break;
                case 4:
                    XmlConfigSave.Change("speed/game", 512000);
                    break;
                case 5:
                    XmlConfigSave.Change("speed/game", 1000000);
                    break;
                case 6:
                    XmlConfigSave.Change("speed/game", 2000000);
                    break;
                case 7:
                    XmlConfigSave.Change("speed/game", 3000000);
                    break;
                case 8:
                    XmlConfigSave.Change("speed/game", 5000000);
                    break;
                case 9:
                    XmlConfigSave.Change("speed/game", 10000000);
                    break;
                case 10:
                    XmlConfigSave.Change("speed/game", 25000000);
                    break;
            }
        }
    }
}