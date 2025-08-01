﻿using Launcher.View.Resources.Script;
using Launcher.View.Resources.Script.Cookie;
using Npgsql;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace Launcher.View.Pages
{
    public partial class AuthorizationPage : Page
    {
        public AuthorizationPage()
        {
            InitializeComponent();
            Update.UpdateUI(BackgroundUIFunction, 0, 0, 1);
        }
        private void BackgroundUIFunction(object sender, EventArgs ea)
        {
            if (Name_AuthorizationPage.Text != string.Empty && Passwordbox_AuthorizationPage.Password != string.Empty) Login.IsEnabled = true;
            else Login.IsEnabled = false;
        }

        private void NotAccount(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Пустышка", "Пустышка");
        }

        private void LoginClick(object sender, RoutedEventArgs e)
        {
            string Username = Name_AuthorizationPage.Text;
            string Password = Passwordbox_AuthorizationPage.Password;

            if (Internet.connect())
            {
                try
                {
                    string query = "SELECT id, password, avatar FROM public.\"user\" WHERE login = @username";

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
                                    double dbid = reader.GetFieldValue<double>(0);
                                    string dbpassword = reader.GetString(1);
                                    string dbavatar = reader.IsDBNull(2) ? null : reader.GetString(1);

                                    if (dbpassword == Password)
                                    {
                                        string paths = Path.Combine(Paths.userdata, Username);

                                        if(!Directory.Exists(paths)) Directory.CreateDirectory(paths);
                                        if(!Directory.Exists(Paths.avatarcache)) Directory.CreateDirectory(Paths.avatarcache);

                                        try
                                        {
                                            Directory.CreateDirectory(Paths.avatarcache);
                                            ManagerCookie.SaveLoginCookie(dbid, Username, Guid.NewGuid().ToString(), DateTime.Now.AddDays(7));

                                            var data = new
                                            {
                                                path = Paths.cookie(Username)
                                            };

                                            File.WriteAllText(Path.Combine(Paths.userdata, "config.json"), JsonSerializer.Serialize(data));
                                        }
                                        catch (Exception ex) {
                                            Loges.LoggingProcess(level: LogLevel.WARN, ex: ex);
                                        }

                                        if (!string.IsNullOrEmpty(dbavatar) && Uri.IsWellFormedUriString(dbavatar, UriKind.Absolute))
                                        {
      /*                                      try
                                            {
                                                DownloadContent.Download(Paths.destinationPath(Paths.avatarcache, Username, "png"), dbavatar);
                                            }
                                            catch (Exception ex)
                                            {
                                                Loges.LoggingProcess(LogLevel.INFO, ex: ex);
                                            }*/
                                        }

                                        ErrorLoginOrPassword_AuthorizationPage.Visibility = Visibility.Hidden;

                                        Window parentWindow = Window.GetWindow(this);
                                        if (parentWindow != null)
                                        {
                                            parentWindow.Close();
                                        }
                                    }
                                    else
                                    {
                                        ErrorLoginOrPassword_AuthorizationPage.Visibility = Visibility.Visible;
                                    }
                                }
                                else
                                {
                                    ErrorLoginOrPassword_AuthorizationPage.Visibility = Visibility.Visible;
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
