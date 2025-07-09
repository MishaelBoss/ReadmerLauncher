using Launcher.View.Components;
using Launcher.View.Resources.Script;
using Npgsql;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Launcher.View.Pages
{
    public partial class HomePage : Page
    {
        private List<CartGame> btnGame = new List<CartGame>();

        public HomePage()
        {
            InitializeComponent();
            ConectToBD();
            Load();
        }

        private void ConectToBD()
        {
            try {
                string sql = "SELECT * FROM public.game";

                using (var conn = new NpgsqlConnection(Arguments.connection))
                {
                    conn.Open();
                    using (var command = new NpgsqlCommand(sql, conn))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string title = reader.GetString(3);
                                string description = reader.GetString(4);
                                string version = reader.GetString(5);
                                string background_image = reader.GetString(6);
                                string icon_image = reader.GetString(7);
                                string header_image = reader.GetString(8);
                                string mini_image = reader.GetString(9);
                                decimal money = reader.GetFieldValue<decimal>(10);
                                string url_download = reader.GetString(11);
                                bool windows_system = reader.GetBoolean(12);
                                bool linux_system = reader.GetBoolean(13);
                                bool mac_system = reader.GetBoolean(14);

                                btnGame.Add(new CartGame()
                                {
                                    SetIcon = new BitmapImage(new Uri(background_image)),
                                    SetName = title
                                });
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

        private void Load()
        {
            foreach (CartGame button in btnGame)
            {
                HomeMainContent.Children.Add(button);
            }
        }
    }
}
