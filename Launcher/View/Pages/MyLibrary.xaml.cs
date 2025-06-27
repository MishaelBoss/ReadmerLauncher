using Launcher.View.Components;
using Launcher.View.Resources.Script;
using Npgsql;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Launcher.View.Pages
{
    public partial class MyLibrary : Page
    {
        List<ButtonSelectGame> btnGame;

        public MyLibrary()
        {
            InitializeComponent();
            ConectToBD();
            Load();
        }

        private void ConectToBD() {

            try {
                //string request = "SELECT * FROM public.library";
                string sql = "SELECT * FROM public.game";

                using (var conn = new NpgsqlConnection(Arguments.connection))
                {
                    conn.Open();
                    using (var command = new NpgsqlCommand(sql, conn))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            btnGame = new List<ButtonSelectGame>();

                            while (reader.Read())
                            {
                                string title = reader.GetString(3);
                                string icon_image = reader.GetString(7);

                                btnGame.Add(new ButtonSelectGame()
                                {
                                    SetIcon = new BitmapImage(new Uri(icon_image)),
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

        private void Load() {
            foreach (ButtonSelectGame button in btnGame) {
                LeftBorder.Children.Add(button);
            }
        }
    }
}