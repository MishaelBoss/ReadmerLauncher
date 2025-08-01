﻿using Launcher.Model;
using Launcher.View.Components;
using Launcher.View.Resources.Script;
using Launcher.View.Resources.Script.Game;
using Launcher.View.Resources.Script.Json;
using Newtonsoft.Json.Linq;
using Npgsql;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Launcher.View.Pages
{
    public partial class GameDetailsPage : Page
    {
        private double _id;
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
                BackgroundGameDetailsPage.Source = new BitmapImage(new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Paths.librarycache, _name, $"{_name}_Background.png"), UriKind.Absolute));
            }
            catch (FileNotFoundException ex)
            {
                Loges.LoggingProcess(
                    level: LogLevel.ERROR,
                    ex: ex
                );
            }
            catch (DirectoryNotFoundException ex)
            {
                Loges.LoggingProcess(
                    level: LogLevel.WARN,
                    ex: ex
                );
            }
            catch (Exception ex)
            {
                Loges.LoggingProcess(
                    level: LogLevel.ERROR,
                    ex: ex
                );
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
                    string requestGame = "SELECT id, background_image, icon_image FROM public.game WHERE title = @namegame";

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
                                    double id = reader.GetDouble(0);
                                    string background_image = reader.GetString(1);
                                    string icon_image = reader.GetString(2);

                                    _id = id;
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
        public async Task DownloadGame(string url)
        {
            if (cancelTokenSource == null || cancelTokenSource.IsCancellationRequested) cancelTokenSource = new CancellationTokenSource();

            CancellationToken token = cancelTokenSource.Token;

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    using (var s = await client.GetStreamAsync(new Uri(""), token))
                    {
                        using (var fs = new FileStream(Paths.download, FileMode.Create))
                        {
                            await s.CopyToAsync(fs);
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    Loges.LoggingProcess(
                        level: LogLevel.ERROR, 
                        ex: ex
                    );
                }
                catch (OperationCanceledException ex) when (cancelTokenSource.IsCancellationRequested)
                {
                    Loges.LoggingProcess(
                        level: LogLevel.INFO, 
                        ex: ex
                    );
                }
                catch (Exception ex) { 
                    Loges.LoggingProcess(
                        level: LogLevel.WARN, 
                        ex: ex
                    );
                }
            }
        }*/

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            if (btn.Content.ToString() == "Запустить игру")
            {

            }
            else
            {
                var installWindow = InformationInstallGame.Instance;
                installWindow.id = _id;
                installWindow.SetName = _name;
                installWindow.SetSize = "1323 MB";
                try
                {
                    installWindow.SetIcon = new BitmapImage(new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Paths.librarycache, _name, $"{_name}_Background.png"), UriKind.Absolute));
                }
                catch (FileNotFoundException ex)
                {
                    Loges.LoggingProcess(
                        level: LogLevel.ERROR,
                        ex: ex
                    );

                    installWindow.SetIcon = new BitmapImage(new Uri(Path.Combine("pack://application:,,,/View/Resources/Image/NotImage.png", _name, $"{_name}_Background.png"), UriKind.Absolute));
                }
                catch (DirectoryNotFoundException ex)
                {
                    Loges.LoggingProcess(
                        level: LogLevel.WARN,
                        ex: ex
                    );

                    installWindow.SetIcon = new BitmapImage(new Uri(Path.Combine("pack://application:,,,/View/Resources/Image/NotImage.png", _name, $"{_name}_Background.png"), UriKind.Absolute));
                }
                catch (Exception ex)
                {
                    Loges.LoggingProcess(
                        level: LogLevel.ERROR,
                        ex: ex
                    );
                }
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
                ManagerLibraryJson.CreateLibraryFolders();
            }
        }
    }
}
