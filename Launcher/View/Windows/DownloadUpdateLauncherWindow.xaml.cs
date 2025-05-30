using CheckConnectInternet;
using Launcher.View.Pages;
using LauncherLes1.View.Resources.Script;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Launcher.View.Windows
{
    public partial class DownloadUpdateLauncherWindow : Window
    {
        public DownloadUpdateLauncherWindow()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize() {
            ShowInTaskbar = false;
            Topmost = true;
            Task task = DownloadFile();
        }

        #region dragMove
        private void WindowMove(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        #endregion

        CancellationTokenSource cancelTokenSource = new CancellationTokenSource();

        private async Task DownloadFile()
        {
            CancellationToken token = cancelTokenSource.Token;

            if (token.IsCancellationRequested)
                return;

            await Task.Delay(1000);

            await Task.Run(async () =>
            {
                using (WebClient wc = new WebClient())
                {
                    if (Internet.connect())
                    {
                        if (!string.IsNullOrEmpty(Paths.zipPathUpdate) && File.Exists(Paths.zipPathUpdate)) File.Delete(Paths.zipPathUpdate);
                        else if (!string.IsNullOrEmpty(Paths.exeLauncherUpdate) && File.Exists(Paths.exeLauncherUpdate)) File.Delete(Paths.exeLauncherUpdate);
                        try
                        {
                            Uri uri = new Uri(Settings.fileDownload);
                            wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressMessageHandler_HttpReceiveProgress);
                            await wc.DownloadFileTaskAsync(uri, "UpdateLauncher.zip");
                            ZipFile.ExtractToDirectory("UpdateLauncher.zip", Paths.exetraPath);
                            File.Delete("UpdateLauncher.zip");

                            CmdClass.Cmd($"taskkill /f /im \"{Arguments.exenames}\" && timeout /t 1 && del \"{Arguments.execPath}\" && ren NewLauncher.exe \"{Arguments.exenames}\" && \"{Arguments.execPath}\"");
                        }
                        catch (OperationCanceledException)
                        {
                            MessageBox.Show("Загрузка была отменена.", "Отмена", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ошибка при загрузке файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ошибка", "Подключитесь к интернету", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }, token);
        }

        private void ProgressMessageHandler_HttpReceiveProgress(object sender, DownloadProgressChangedEventArgs e)
                   => DownloadProgressCallback(sender, e, LabelContentDownload);

        private static void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e, Label text)
        {
            string message = $"{e.BytesReceived} загружено из {e.TotalBytesToReceive} байт. {e.ProgressPercentage} % завершено...";
            text.Dispatcher.Invoke(() => text.Content = message);
        }

        private void ButtonCancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
            cancelTokenSource.Cancel();

        }
    }
}
