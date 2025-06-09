using CheckConnectInternet;
using GroupDocs.Conversion;
using Launcher.View.Components;
using Launcher.View.Resources.Script;
using Launcher.View.Resources.Script.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Launcher.View.Pages
{
    public partial class Game : Page
    {
        private string _name { get; set; }
        private string _version { get; set; }
        private string _description { get; set; }
        private string _icon { get; set; }
        private int _watch { get; set; }
        private int _wereTime { get; set; }

        private bool? isFileDownloadNow { get; set; } = false;

        private bool initializeParametr()
        {
            if (_name != string.Empty && _version != string.Empty) {
                try
                {
                    return true;
                }
                catch (Exception ex)
                {
                    Loges.LoggingProcess(LogLevel.WARN, ex: ex);
                    return false;
                }
            }
            else {
                return false; 
            }
        }

        private Stopwatch watch = new Stopwatch();

        public Game()
        {
            InitializeComponent();
            LoadData();
            Initialize();
        }

        private async Task DownloadIcon()
        {
            try
            {
                await Task.Run(async () =>
                {
                    if (Internet.connect())
                    {
                        string destinationPath = Path.Combine(Paths.games, $"{_name}.png");
                        string convertPath = Path.Combine(Paths.games, $"{_name}.ico");
                        if (!File.Exists(destinationPath) && !File.Exists(convertPath)) {
                            using (WebClient wc = new WebClient())
                            {
                                try
                                {
                                    Uri uri = new Uri(_icon);
                                    await wc.DownloadFileTaskAsync(uri, destinationPath);
                                    if(!File.Exists(convertPath))
                                        FluentConverter.Load(destinationPath).ConvertTo(convertPath).Convert();

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
                    }
                    else
                    {
                        Loges.LoggingProcess(LogLevel.INFO, "Подключитесь к интернету");
                        MessageBox.Show("Ошибка", "Подключитесь к интернету", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                });
            }
            catch (Exception ex)
            {
                Loges.LoggingProcess(LogLevel.INFO, ex: ex);
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Initialize() {
            CheckInstallation();

            if (initializeParametr())
            {
                Update.UpdateUI(BackgroundUIFunction, 0, 0, 1);

                string AppmanifestFilePath = Path.Combine(Paths.work, $"appmanifest_{_name}.json");
                if (File.Exists(AppmanifestFilePath))
                {

                }
                else
                {

                }
            }
            else
            {

            }
        }

        private void LoadData() {
            _name = "DefenderRat";
            _version = "1.0.0.0";
            _icon = "https://raw.githubusercontent.com/RedmerGameAndTechnologies/JsonLauncher/refs/heads/main/icons/DefenderRat/DefenderRat_icon.png";

            DownloadIcon();
        }

        private void BackgroundUIFunction(object sender, EventArgs ea)
        {

        }

        CancellationTokenSource cancelTokenSource;
        private void DownloadGame() {
            /*try
            {
                if (cancelTokenSource == null || cancelTokenSource.IsCancellationRequested) cancelTokenSource = new CancellationTokenSource();
                CancellationToken token = cancelTokenSource.Token;

                Task.Run(async () => {
                    HttpRequestMessage httpRequestMessage = new HttpRequestMessage() { Method = HttpMethod.Get, RequestUri = new Uri(updateContent.urlDownload) };
                    ProgressMessageHandler progressMessageHandler = new ProgressMessageHandler(new HttpClientHandler() { AllowAutoRedirect = true });
                    httpClient = new HttpClient(progressMessageHandler) { Timeout = Timeout.InfiniteTimeSpan };
                    watch.Start();
                    progressMessageHandler.HttpReceiveProgress += ProgressMessageHandler_HttpReceiveProgress;
                    Stream streamFileServer = await httpClient.GetStreamAsync(httpRequestMessage.RequestUri);
                    Stream fileStreamServer = new FileStream(zipPath, FileMode.OpenOrCreate, FileAccess.Write);
                    try
                    {
                        await streamFileServer.CopyToAsync(fileStreamServer, Arguments.Speed_download_game, cancellationToken: token);
                        cancelTokenSource.Dispose();
                        streamFileServer.Dispose();
                        fileStreamServer.Dispose();
                        return;
                    }
                    catch (Exception e)
                    {
                        DownloadAppState.Dispatcher.Invoke(() => DownloadAppState.Text = "State: " + e.Message.ToString());
                        LaunchGameButton.Dispatcher.Invoke(() => LaunchGameButton.IsEnabled);
                        cancelTokenSource.Dispose();
                        streamFileServer.Dispose();
                        fileStreamServer.Dispose();
                        return;
                    }
                }, cancellationToken: token);
            }
            catch (Exception ex) {
                Loges.LoggingProcess(LogLevel.ERROR, ex: ex);
            }*/
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            if (btn.Content.ToString() == "Запустить игру")
            {

            }
            else
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(@"https://raw.githubusercontent.com/RedmerGameAndTechnologies/JsonLauncher/refs/heads/main/icons/DefenderRat/Defender%20Rat.png");
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                var installWindow = InformationInstallGame.Instance;
                installWindow.SetName = _name;
                installWindow.SetIcon = bitmapImage;
                installWindow.SetSize = "1323 MB";
                installWindow.Visibility = Visibility.Visible;
            }
        }

        public void CheckInstallation()
        {
            string LibraryfoldersFilePath = Path.Combine(Paths.work, Files.LibraryfoldersJson);

            if (File.Exists(LibraryfoldersFilePath))
            {
                try
                {
                    string json = File.ReadAllText(LibraryfoldersFilePath);
                    var data = JObject.Parse(json);

                    string manifestPath = data["apps"]?[_name]?.ToString();
                    if (string.IsNullOrEmpty(manifestPath))
                    {
                        btn.Content = "Скачать игру";
                        return;
                    }

                    string manifestJson = File.ReadAllText(manifestPath);
                    var manifest = JObject.Parse(manifestJson);
                    string gamePath = manifest["install_dir"]?.ToString();

                    if (!string.IsNullOrEmpty(gamePath) && Directory.Exists(gamePath)) btn.Content = "Запустить игру";
                    else btn.Content = "Скачать игру";
                }
                catch (Exception ex) {
                    Loges.LoggingProcess(LogLevel.ERROR, ex: ex);
                    btn.Content = "Скачать игру";
                }
            }
            else {
                JsonConfidCreate.CreateLibraryFolders(LibraryfoldersFilePath);
            }
        }
    }
}
