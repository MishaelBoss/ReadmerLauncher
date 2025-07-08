using CommunityToolkit.Mvvm.Input;
using Launcher.Model;
using Launcher.View.Components;
using Launcher.View.Resources.Script;
using Launcher.View.Resources.Script.Game;
using Launcher.View.Resources.Script.Json;
using Newtonsoft.Json.Linq;
using Npgsql;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Launcher.View.Pages
{
    public partial class MyLibraryPage : Page
    {
        List<Game> gamesList;
        List<ButtonSelectGame> btnGame;

        public MyLibraryPage()
        {
            InitializeComponent();
            CheckInstallGame();
            if (Internet.connect()) ConectToBD(4);
            else Loges.LoggingProcess(LogLevel.INFO, "Подключитесь к интернет");
        }

        private void CheckInstallGame() {
            string LibraryfoldersFilePath = Path.Combine(Paths.work, Files.LibraryfoldersJson);

            if (File.Exists(LibraryfoldersFilePath))
            {
                try
                {
                    btnGame = [];
                    gamesList = [];

                    string json = File.ReadAllText(LibraryfoldersFilePath);
                    var data = JObject.Parse(json);
                    var apps = data["apps"] as JObject;

                    foreach (var app in apps.Properties()) {
                        string manifestPath = app.Value?.ToString();

                        if (File.Exists(manifestPath))
                        {
                            var manifest = JObject.Parse(File.ReadAllText(manifestPath));
                            string _name = manifest["name"]?.ToString();
                            string icon = manifest["icon"]?.ToString();
                            string background = manifest["background"]?.ToString();

                            var game = new Game()
                            {
                                _name = _name,
                                _icon = icon,
                                _background = background
                            };

                            gamesList.Add(game);

                            if (!string.IsNullOrEmpty(manifestPath))
                            {
                                if (!File.Exists(Paths.destinationPath(Paths.appLibrarycache(name: _name), name: $"{_name}_Icon", extension: "png"))) {
                                    if (!Directory.Exists(Paths.appLibrarycache(name: _name)))
                                    {
                                        try
                                        {
                                            Directory.CreateDirectory(Paths.appLibrarycache(name: _name));
                                        }
                                        catch (Exception ex)
                                        {
                                            Loges.LoggingProcess(LogLevel.WARN, $"Не удалось создать папку для {_name}", ex);
                                        }
                                    }

                                    if (Directory.Exists(Paths.appLibrarycache(name: _name)))
                                    {
                                        if (Internet.connect())
                                        {
                                            try
                                            {
                                                string requestGame = "SELECT icon_image FROM public.game WHERE title = @namegame";

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
                                                                string icon_image = reader.GetString(0);

                                                                try
                                                                {
                                                                    if (!File.Exists(Paths.destinationPath(Paths.appLibrarycache(name: _name), name: $"{_name}_Icon", extension: "png"))) DownloadContent.Download(Paths.destinationPath(Paths.appLibrarycache(_name), $"{_name}_Icon", "png"), icon_image);
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    Loges.LoggingProcess(LogLevel.WARN, $"Не удалось загрузить иконку для {_name}", ex);
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
                                }

                                if (File.Exists(Paths.destinationPath(Paths.appLibrarycache(name: _name), name: $"{_name}_Icon", extension: "png"))) {
                                    string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Paths.librarycache, _name, $"{_name}_Icon.png");
                                    btnGame.Add(new ButtonSelectGame()
                                    {
                                        Command = new RelayCommand(() => NavigateToGameDetails(game)),
                                        SetIcon = new BitmapImage(new Uri(fullPath, UriKind.Absolute)),
                                        SetName = _name,
                                        SetOpacityName = 1
                                    });
                                }
                            }
                        }
                    }

                    Load();
                }
                catch (Exception ex)
                {
                    Loges.LoggingProcess(LogLevel.ERROR, ex: ex);
                }
            }
            else
            {
                JsonConfidCreate.CreateLibraryFolders();
            }
        }

        private void ConectToBD(decimal Id) {
            try {
                btnGame = [];
                gamesList = [];

                string requestUser = "SELECT * FROM public.library WHERE user_id = @Id";
                string requestGame = "SELECT * FROM public.game WHERE id = @game_id";

                using (var conn = new NpgsqlConnection(Arguments.connection))
                {
                    conn.Open();
                    using (var command = new NpgsqlCommand(requestUser, conn))
                    {
                        command.Parameters.AddWithValue("@Id", Id);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                decimal game_id = reader.GetFieldValue<decimal>(1);

                                using (var conn2 = new NpgsqlConnection(Arguments.connection))
                                {
                                    conn2.Open();
                                    using (var command2 = new NpgsqlCommand(requestGame, conn2))
                                    {
                                        command2.Parameters.AddWithValue("@game_id", game_id);

                                        using (var reader2 = command2.ExecuteReader())
                                        {
                                            while (reader2.Read())
                                            {
                                                string title = reader2.GetString(3);
                                                string icon_image = reader2.GetString(7);

                                                var game = new Game()
                                                {
                                                    _name = title,
                                                    _icon = icon_image
                                                };

                                                gamesList.Add(game);

                                                btnGame.Add(new ButtonSelectGame()
                                                {
                                                    Command = new RelayCommand(() => NavigateToGameDetails(game)),
                                                    SetIcon = new BitmapImage(new Uri(icon_image)),
                                                    SetName = title,
                                                    SetOpacityName = 0.3
                                                });
                                                Load();
                                            }
                                        }
                                    }
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

        private void NavigateToGameDetails(Game selectedGame)
        {
            LibraryFrame.Navigate(new GameDetailsPage(selectedGame));
        }

        private void Load() {
            foreach (ButtonSelectGame button in btnGame) {
                LeftBorder.Children.Add(button);
            }
        }
    }
}