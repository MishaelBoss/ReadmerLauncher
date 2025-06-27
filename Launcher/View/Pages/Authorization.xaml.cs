using Launcher.View.Resources.Script;
using Launcher.View.Resources.Script.Cookie;
using Launcher.View.Resources.Script.Game;
using Npgsql;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Launcher.View.Pages
{
    public partial class Authorization : Page
    {
        public Authorization()
        {
            InitializeComponent();
            Update.UpdateUI(BackgroundUIFunction, 0, 0, 1);
        }
        private void BackgroundUIFunction(object sender, EventArgs ea)
        {
            if (Name.Text != string.Empty && Passwordbox.Password != string.Empty) Login.IsEnabled = true;
            else Login.IsEnabled = false;
        }

        private void NotAccount(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Пустышка", "Пустышка");
        }

        private void isRememberClicked(object sender, RoutedEventArgs e)
        {
            var isRemember = Remember.IsChecked ?? false;
        }

        private void LoginClick(object sender, RoutedEventArgs e)
        {
            string Username = Name.Text;
            string Password = Passwordbox.Password;

            if (Internet.connect())
            {
                try
                {
                    string query = "SELECT password, avatar FROM public.\"user\" WHERE login = @username";

                    using (var connection = new NpgsqlConnection(Arguments.connection))
                    {
                        connection.Open();
                        using (var command = new NpgsqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@username", Username);

                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    string dbpassword = reader.GetString(0);
                                    string dbavatar = reader.IsDBNull(1) ? null : reader.GetString(1);

                                    if (dbpassword == Password)
                                    {
                                        string paths = Path.Combine(Paths.userdata, Username);

                                        if (Directory.Exists(paths) && Directory.Exists(Paths.avatarcache))
                                        {
                                            Directory.CreateDirectory(Paths.avatarcache);
                                            CrateCookie.SaveLoginCookie(Username, Guid.NewGuid().ToString(), DateTime.Now.AddDays(7));
                                        }
                                        else
                                        {
                                            Directory.CreateDirectory(Paths.avatarcache);
                                            Directory.CreateDirectory(paths);
                                            CrateCookie.SaveLoginCookie(Username, Guid.NewGuid().ToString(), DateTime.Now.AddDays(7));
                                        }

                                        if (!string.IsNullOrEmpty(dbavatar) && Uri.IsWellFormedUriString(dbavatar, UriKind.Absolute))
                                        {
                                            try
                                            {
                                                DownloadContent.Download(Paths.destinationPath(Paths.avatarcache, Username, "png"), dbavatar);
                                            }
                                            catch (Exception ex)
                                            {
                                                Loges.LoggingProcess(LogLevel.INFO, ex: ex);
                                            }
                                        }

                                        ErrorLoginOrPassword.Visibility = Visibility.Hidden;

                                        Window parentWindow = Window.GetWindow(this);
                                        if (parentWindow != null)
                                        {
                                            parentWindow.Close();
                                        }
                                    }
                                    else
                                    {
                                        ErrorLoginOrPassword.Visibility = Visibility.Visible;
                                    }
                                }
                                else
                                {
                                    ErrorLoginOrPassword.Visibility = Visibility.Visible;
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
                Loges.LoggingProcess(LogLevel.WARN, "Подключитесь к интернету");
            }
        }
    }
}
