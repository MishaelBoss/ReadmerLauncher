using Launcher.Model;
using Launcher.View.Components;
using Launcher.View.Resources.Script;
using Launcher.View.Resources.Script.Game;
using Launcher.View.Resources.Script.Json;
using Newtonsoft.Json.Linq;
using Npgsql;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Launcher.View.Pages
{
    public partial class GameDetailsPage : Page
    {
        private string _name;
        private string _version;
        private string _icon;
        private string _background;
        private int _watch;
        private int _wereTime;
        private bool? isFileDownloadNow;

        private Stopwatch watch = new();

        public GameDetailsPage(Game game)
        {
            InitializeComponent();
            DataContext = game;
            LoadData(game);
            Initialize();
        }

        private void Initialize() {
            CheckInstallation();

            try
            {
                Background.Source = new BitmapImage(new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Paths.librarycache, _name, $"{_name}_Background.png"), UriKind.Absolute));
            }
            catch (Exception ex)
            {
                Loges.LoggingProcess(LogLevel.ERROR, ex: ex);
            }

            string AppmanifestFilePath = Path.Combine(Paths.work, $"appmanifest_{_name}.json");
            if (File.Exists(AppmanifestFilePath))
            {

            }
            else
            {

            }

            Update.UpdateUI(BackgroundUIFunction, 0, 0, 1);
        }

        private void LoadData(Game game) {
            _name = game._name;

            if (Internet.connect())
            {
                try
                {
                    string requestGame = "SELECT background_image, icon_image FROM public.game WHERE title = @namegame";

                    using (var conn = new NpgsqlConnection(Arguments.connection))
                    {
                        conn.Open();
                        using (var command = new NpgsqlCommand(requestGame, conn))
                        {
                            command.Parameters.AddWithValue("@namegame", _name);

                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string background_image = reader.GetString(0);
                                    string icon_image = reader.GetString(1);

                                    _background = background_image;
                                    _icon = icon_image;

                                    if(!Directory.Exists(Paths.appcache)) Directory.CreateDirectory(Paths.appcache);
                                    if(!Directory.Exists(Paths.librarycache)) Directory.CreateDirectory(Paths.librarycache);

                                    if (!Directory.Exists(Paths.appLibrarycache(name: _name))) {
                                        try
                                        {
                                            Directory.CreateDirectory(Paths.appLibrarycache(name: _name));
                                        }
                                        catch (Exception ex)
                                        {
                                            Loges.LoggingProcess(LogLevel.WARN, $"Не удалось создать папку для {_name}", ex);
                                        }
                                    }

                                    if (Directory.Exists(Paths.appcache) && Directory.Exists(Paths.librarycache) && Directory.Exists(Paths.appLibrarycache(name: _name)))
                                    {
                                        if(!File.Exists(Paths.destinationPath(Paths.appLibrarycache(name: _name), name: $"{_name}_Background", extension: "png"))){
                                            try
                                            {
                                                DownloadContent.Download(Paths.destinationPath(Paths.appLibrarycache(_name), $"{_name}_Background", "png"), _background);
                                            }
                                            catch (Exception ex)
                                            {
                                                Loges.LoggingProcess(LogLevel.WARN, $"Не удалось загрузить фон для {_name}", ex);
                                            }
                                        }

                                        if (!File.Exists(Paths.destinationPath(Paths.appLibrarycache(name: _name), name: $"{_name}_Icon", extension: "png"))) {
                                            try
                                            {
                                                DownloadContent.Download(Paths.destinationPath(Paths.appLibrarycache(_name), $"{_name}_Icon", "png"), _icon);
                                            }
                                            catch (Exception ex)
                                            {
                                                Loges.LoggingProcess(LogLevel.WARN, $"Не удалось загрузить мини иконку для {_name}", ex);
                                            }
                                        }

                                        if (!!File.Exists(Paths.destinationPath(Paths.games, name: _name, extension: "ico"))) {
                                            if (!Directory.Exists(Paths.readmer)) Directory.CreateDirectory(Paths.readmer);
                                            if (!Directory.Exists(Paths.games)) Directory.CreateDirectory(Paths.games);

                                            try
                                            {
                                                DownloadContent.Download(Paths.destinationPath(Paths.games, _name, "ico"), _icon);
                                            }
                                            catch (Exception ex)
                                            {
                                                Loges.LoggingProcess(LogLevel.WARN, $"Не удалось загрузить иконку для {_name}", ex);
                                            }
                                        }
                                    }
                                    else {
                                        Loges.LoggingProcess(level: LogLevel.ERROR, "Нет директори");
                                    }
                                }
                            }
                        }
                    }
                }
                catch (NpgsqlException ex)
                {
                    Loges.LoggingProcess(LogLevel.WARN, "Ошибка подключения или запроса", ex: ex);
                }
            }
            else
            {
                Loges.LoggingProcess(LogLevel.INFO, "Подключитесь к интернет");
            }
        }

        private void BackgroundUIFunction(object sender, EventArgs ea)
        {

        }

        /*CancellationTokenSource cancelTokenSource;
        private void DownloadGame() {
            try
            {
                if (cancelTokenSource == null || cancelTokenSource.IsCancellationRequested) cancelTokenSource = new CancellationTokenSource();
                CancellationToken token = cancelTokenSource.Token;

                Task.Run(async () =>
                {
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
            catch (Exception ex)
            {
                Loges.LoggingProcess(LogLevel.ERROR, ex: ex);
            }
        }*/

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            if (btn.Content.ToString() == "Запустить игру")
            {

            }
            else
            {
                try
                {
                    var installWindow = InformationInstallGame.Instance;
                    installWindow.SetName = _name;
                    installWindow.SetIcon = new BitmapImage(new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Paths.librarycache, _name, $"{_name}_Background.png"), UriKind.Absolute));
                    installWindow.SetSize = "1323 MB";
                    installWindow.Visibility = Visibility.Visible;
                }
                catch (Exception ex)
                {
                    Loges.LoggingProcess(LogLevel.ERROR, ex: ex);
                }
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
                JsonConfidCreate.CreateLibraryFolders();
            }
        }
    }
}
