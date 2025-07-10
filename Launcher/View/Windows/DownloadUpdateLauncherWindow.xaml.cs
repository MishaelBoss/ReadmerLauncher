using Launcher.View.Resources.Script;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Handlers;
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

            if (Arguments.isEmergencyUpdate) Button.Visibility = Visibility.Hidden;
            else Button.Visibility = Visibility.Visible;

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
                    var watch = new Stopwatch();

                    if (Internet.connect())
                    {
                        using (var client = new HttpClient())
                        {
                            string path = Paths.destinationPath(Paths.download, "UpdateLauncher", "zip");

                            if (!string.IsNullOrEmpty(Paths.destinationPath(Paths.download, "UpdateLauncher", "zip")) && File.Exists(Paths.destinationPath(Paths.download, "UpdateLauncher", "zip"))) File.Delete(Paths.destinationPath(Paths.download, "UpdateLauncher", "zip"));
                            else if (!string.IsNullOrEmpty(Paths.destinationPath(Paths.download, "NewLauncher", "exe")) && File.Exists(Paths.destinationPath(Paths.download, "NewLauncher", "exe"))) File.Delete(Paths.destinationPath(Paths.download, "NewLauncher", "exe"));

                            try
                            {
                                using (var s = await client.GetStreamAsync(new Uri(Arguments.fileDownloadLink), token))
                                {
                                    using (var fs = new FileStream(path, FileMode.Create))
                                    {
                                        await s.CopyToAsync(fs);
                                    }
                                }

                                if (File.Exists(path) && !token.IsCancellationRequested)
                                {
                                    ZipFile.ExtractToDirectory(path, AppDomain.CurrentDomain.BaseDirectory);
                                    File.Delete(path);

                                    CmdClass.Cmd($"taskkill /f /im \"{Arguments.exenames}\" && timeout /t 1 && del \"{Arguments.execPath}\" && ren NewLauncher.exe \"{Arguments.exenames}\" && \"{Arguments.execPath}\"");
                                }
                            }
                            catch (HttpRequestException ex)
                            {
                                Loges.LoggingProcess(
                                    level: LogLevel.ERROR, 
                                    ex: ex
                                );
                                MessageBox.Show($"Message :{ex.Message}\nИсключение!");
                            }
                            catch (OperationCanceledException ex) when (ex.CancellationToken.IsCancellationRequested)
                            {
                                Loges.LoggingProcess(
                                    level: LogLevel.INFO, 
                                    ex: ex
                                );
                                MessageBox.Show("Загрузка была отменена.", "Отмена", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            catch (Exception ex)
                            {
                                Loges.LoggingProcess(
                                    level: LogLevel.WARN, 
                                    ex: ex
                                );
                                MessageBox.Show($"Ошибка при загрузке контента: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                    else
                    {
                        Loges.LoggingProcess(LogLevel.INFO, "Подключитесь к интернету");
                        MessageBox.Show("Вовремя работы отключился интернет", "Подключитесь к интернету", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                });
            }
            catch (Exception ex)
            {
                Loges.LoggingProcess(LogLevel.ERROR, ex: ex);
            }

            /*try
            {
                if (cancelTokenSource == null || cancelTokenSource.IsCancellationRequested) cancelTokenSource = new CancellationTokenSource();

                CancellationToken token = cancelTokenSource.Token;

                await Task.Delay(1000);

                await Task.Run(async () =>
                {
                    var watch = new Stopwatch();
                    if (Internet.connect())
                    {
                        using (WebClient wc = new WebClient())
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
                                Loges.LoggingProcess(LogLevel.INFO, ex: ex);
                                MessageBox.Show("Загрузка была отменена.", "Отмена", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            catch (Exception ex)
                            {
                                Loges.LoggingProcess(LogLevel.ERROR, ex: ex);
                                MessageBox.Show($"Ошибка при загрузке файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                    else
                    {
                        Loges.LoggingProcess(LogLevel.INFO, "Подключитесь к интернету");
                        MessageBox.Show("Ошибка", "Подключитесь к интернету", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }, token);
            }
            catch (Exception ex)
            {
                Loges.LoggingProcess(LogLevel.INFO, ex: ex);
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }*/
        }

        private void ProgressMessageHandler_HttpReceiveProgressHttp(object sender, HttpProgressEventArgs e)
           => DownloadProgressCallbackHttp(sender, e, LabelContentDownload);

/*        private void ProgressMessageHandler_HttpReceiveProgress(object sender, DownloadProgressChangedEventArgs e)
                   => DownloadProgressCallback(sender, e, LabelContentDownload);

        private static void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e, Label text)
        {
            string message = $"{e.BytesReceived} загружено из {e.TotalBytesToReceive} байт. {e.ProgressPercentage} % завершено... \n {Arguments.Speed_download_update}";
            text.Dispatcher.Invoke(() => text.Content = message);
        }*/

        private static void DownloadProgressCallbackHttp(object sender, HttpProgressEventArgs e, Label text)
        {
            string message = $"{e.BytesTransferred} загружено из {e.TotalBytes.Value} байт. {e.ProgressPercentage} % завершено... \n {Arguments.Speed_download_update}";
            text.Dispatcher.Invoke(() => text.Content = message);
        }

        private void Click(object sender, RoutedEventArgs e)
        {
            cancelTokenSource.Cancel();
            Close();
        }
    }
}
