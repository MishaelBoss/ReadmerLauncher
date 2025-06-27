using Launcher.View.Resources.Script;
using Npgsql;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Launcher.View.Pages
{
    public partial class Profile : Page
    {
        private string _profileUserAvatar;
        private string _profileUserName;
        private string _profileUserLocation;

        public Profile()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize() {
            if (Internet.connect()) {
                LoadData("test2");
            }

            InitializeParametrs();
        }

        private void InitializeParametrs() {
            if (_profileUserAvatar != string.Empty && _profileUserAvatar != null) Avatar.Source = new BitmapImage(new Uri(_profileUserAvatar.ToString()));
            else Avatar.Source = new BitmapImage(new Uri("pack://application:,,,/View/Resources/Image/DefaultAvatar.png"));
            if (_profileUserName != string.Empty && _profileUserName != null) UserName.Content = _profileUserName;
            else UserName.Content = string.Empty;
            if (_profileUserLocation != string.Empty && _profileUserLocation != null) Location.Content = _profileUserLocation;
            else Location.Content = string.Empty;
        }

        private void LoadData(string username) {
            try
            {
                string query = "SELECT login, avatar, location FROM public.\"user\" WHERE login = @username";

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
                                    string dblocation = reader.IsDBNull(2) ? null : reader.GetString(2);

                                    _profileUserName = dblogin;

                                    if (!string.IsNullOrEmpty(dbavatar) && Uri.IsWellFormedUriString(dbavatar, UriKind.Absolute) || !string.IsNullOrEmpty(dblocation))
                                    {
                                        _profileUserAvatar = dbavatar;
                                        _profileUserLocation = dblocation;
                                    }
                                    else
                                    {
                                        _profileUserAvatar = Properties.Resources.DefaultAvatar.ToString();
                                        _profileUserLocation = string.Empty;
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
            catch (Exception ex)
            {
                Loges.LoggingProcess(LogLevel.WARN, ex: ex);
            }
        }
    }
}
