using Launcher.View.Resources.Script;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Launcher.View.Pages
{
    public partial class ProfilePage : Page
    {
        private string _profileUserAvatar;
        private string _profileUserName;
        private string _profileUserLocation;

        public ProfilePage()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize() {
            if (Internet.connect())
            {
                _profileUserAvatar = UserSession.CurrentUser.avatar;
                _profileUserName = UserSession.CurrentUser.Username;
                _profileUserLocation = UserSession.CurrentUser.location;
            }
            else {
                Loges.LoggingProcess(level: LogLevel.INFO, "Подключитесь к интернету");
            }

            InitializeParametrs();
        }

        private void InitializeParametrs() {
            Avatar.Source = new BitmapImage(new Uri(_profileUserAvatar.ToString()));
            UserName.Content = _profileUserName;
            Location.Content = _profileUserLocation;
        }
    }
}
