using CheckConnectInternet;
using Launcher.View.Resources.Script;
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

        CancellationTokenSource cancelTokenSource;

        private async Task DownloadFile()
        {
            try
            {
                if (cancelTokenSource == null || cancelTokenSource.IsCancellationRequested) cancelTokenSource = new CancellationTokenSource();

                CancellationToken token = cancelTokenSource.Token;

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
                                Uri uri = new Uri(Arguments.fileDownloadLink);
                                wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressMessageHandler_HttpReceiveProgress);
                                await wc.DownloadFileTaskAsync(uri, "UpdateLauncher.zip");
                                ZipFile.ExtractToDirectory("UpdateLauncher.zip", Paths.exetraPath);
                                File.Delete("UpdateLauncher.zip");

                                CmdClass.Cmd($"taskkill /f /im \"{Arguments.exenames}\" && timeout /t 1 && del \"{Arguments.execPath}\" && ren NewLauncher.exe \"{Arguments.exenames}\" && \"{Arguments.execPath}\"");
                            }
                            catch (OperationCanceledException ex)
                            {
                                Loges.LoggingProcess(LogLevel.INFO, ex.Message, Loges.GetPageInfo(ex));
                                MessageBox.Show("Загрузка была отменена.", "Отмена", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            catch (Exception ex)
                            {
                                Loges.LoggingProcess(LogLevel.ERROR, ex.Message, Loges.GetPageInfo(ex));
                                MessageBox.Show($"Ошибка при загрузке файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        else
                        {
                            Loges.LoggingProcess(LogLevel.INFO, "Подключитесь к интернету");
                            MessageBox.Show("Ошибка", "Подключитесь к интернету", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }, token);
            }
            catch (Exception ex)
            {
                Loges.LoggingProcess(LogLevel.INFO, ex.Message, Loges.GetPageInfo(ex));
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ProgressMessageHandler_HttpReceiveProgress(object sender, DownloadProgressChangedEventArgs e)
                   => DownloadProgressCallback(sender, e, LabelContentDownload);

        private static void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e, Label text)
        {
            string message = $"{e.BytesReceived} загружено из {e.TotalBytesToReceive} байт. {e.ProgressPercentage} % завершено...";
            text.Dispatcher.Invoke(() => text.Content = message);
        }

        private void Click(object sender, RoutedEventArgs e)
        {
            cancelTokenSource.Cancel();
            Close();
        }
    }
}
