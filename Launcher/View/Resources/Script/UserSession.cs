using Launcher.Model;
using Npgsql;
using System.IO;
using System.Text.Json;

namespace Launcher.View.Resources.Script
{
    public static class UserSession
    {
        public static User CurrentUser { get; private set; }

        public static void Initialize()
        {
            if (Internet.connect())
            {
                string pathConfig = Path.Combine(Paths.userdata, "config.json");

                if (File.Exists(pathConfig))
                {
                    var jsonConfig = File.ReadAllText(pathConfig);
                    var dataConfig = JsonSerializer.Deserialize<Config>(jsonConfig);

                    string cookieFilePath = Path.Combine(dataConfig?.path, "login.cookie");

                    var jsonCookie = File.ReadAllText(cookieFilePath);
                    var dataCookie = JsonSerializer.Deserialize<CookieServer>(jsonCookie);

                    if (File.Exists(Path.Combine(dataConfig?.path, "login.cookie")))
                    {
                        LoadData(dataCookie.id);
                    }
                }
            }
            else
            {
                Loges.LoggingProcess(level: LogLevel.INFO, "Подключитесь к интернету");
            }
        }

        private static void LoadData(double userid)
        {
            try
            {
                string query = "SELECT id, login, avatar, email, number_phone, money, location FROM public.user WHERE id = @Id";

                using (var connection = new NpgsqlConnection(Arguments.connection))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", userid);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                CurrentUser = new User
                                {
                                    id = reader.GetFieldValue<double>(0),
                                    Username = reader.GetString(1),
                                    avatar = reader.IsDBNull(2) ? "pack://application:,,,/View/Resources/Image/DefaultAvatar.png" : reader.GetString(2),
                                    email = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                    number_phone = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                                    money = reader.IsDBNull(5) ? 0 : reader.GetFieldValue<decimal>(5),
                                    location = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Loges.LoggingProcess(LogLevel.WARN, ex: ex);
            }
        }
    }
}
