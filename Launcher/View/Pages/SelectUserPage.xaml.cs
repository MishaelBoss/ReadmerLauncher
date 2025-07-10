using Launcher.View.Components;
using Launcher.View.Resources.Script;
using Launcher.View.Windows;
using Npgsql;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Launcher.View.Pages
{
    public partial class SelectUserPage : Page
    {
        private List<ButtonSelectUser> btnGame = [];
        private List<string> userFolders = [];
        private List<string> folderNames = [];

        public SelectUserPage()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize() {
            btnGame.Add(new ButtonSelectUser()
            {
                Action = () => { OpenAuthorization(); },
                SetIcon = new BitmapImage(new Uri("pack://application:,,,/View/Resources/Image/AddUserImage.png")),
                SetUserName = "Добавить пользователя"
            });

            if (Internet.connect())
            {
                new Thread(() =>
                {
                    try {

                        Application.Current.Dispatcher.BeginInvoke(() =>
                        {
                            SearchFolder();
                        });
                    }
                    catch (Exception ex){
                        Loges.LoggingProcess(
                            level: LogLevel.WARN, 
                            ex: ex
                        );
                    }
                }).Start();
            }
            else
            {
                MessageBox.Show("Подключитесь к интернету");
                Loges.LoggingProcess(LogLevel.INFO, "Подключитесь к интернету");
            }
        }

        private void SearchFolder() {
            foreach (string folder in Directory.GetDirectories(Paths.userdata, "*", SearchOption.AllDirectories))
            {
                folderNames.Add(Path.GetFileName(folder));
                userFolders.Add(folder);
            }

            foreach (string username in folderNames)
            {
                ConnectToDB(username);
            }

            Load();
        }

        private void ConnectToDB(string username) {
            try
            {
                string query = "SELECT login, avatar FROM public.\"user\" WHERE login = @username";

                using (var connection = new NpgsqlConnection(Arguments.connection))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);

                        if (username != null)
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    string dblogin = reader.GetString(0);
                                    string dbavatar = reader.IsDBNull(1) ? null : reader.GetString(1);

                                    if (!string.IsNullOrEmpty(dbavatar) && Uri.IsWellFormedUriString(dbavatar, UriKind.Absolute))
                                    {
                                        btnGame.Add(new ButtonSelectUser()
                                        {
                                            Action = () => { CheckLoginCookie(dblogin); },
                                            SetIcon = new BitmapImage(new Uri(dbavatar)),
                                            SetUserName = dblogin
                                        });
                                    }
                                    else {
                                        btnGame.Add(new ButtonSelectUser()
                                        {
                                            Action = () => { CheckLoginCookie(dblogin); },
                                            SetIcon = new BitmapImage(new Uri("pack://application:,,,/View/Resources/Image/DefaultAvatar.png")),
                                            SetUserName = dblogin
                                        });
                                    }
                                }
                                else
                                {
                                    Loges.LoggingProcess(LogLevel.INFO, "Данный пользователь не найден");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                Loges.LoggingProcess(LogLevel.WARN, ex: ex);
            }
        }

        private void Load()
        {
            foreach (ButtonSelectUser button in btnGame)
            {
                ListUsersContent.Children.Add(button);
            }
        }

        private void CheckLoginCookie(string username) {
            if (File.Exists(Path.Combine(Paths.cookie(username), "login.cookie")))
            {
                try
                {
                    var data = new
                    {
                        path = Paths.cookie(username)
                    };

                    File.WriteAllText(Path.Combine(Paths.userdata, "config.json"), JsonSerializer.Serialize(data));

                    Window parentWindow = Window.GetWindow(this);
                    if (parentWindow != null) parentWindow.Close();
                }
                catch (Exception ex)
                {
                    Loges.LoggingProcess(level: LogLevel.WARN, ex: ex);
                }
            }
            else {
                AuthorizationPage authorization = new();
                AuthorizationWindow.navigationService.Navigate(authorization);
            }
        }

        private void OpenAuthorization() {
            AuthorizationPage authorization = new();
            AuthorizationWindow.navigationService.Navigate(authorization);
        }
    }
}
