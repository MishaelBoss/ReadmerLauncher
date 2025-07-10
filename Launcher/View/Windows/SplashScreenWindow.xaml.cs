using Launcher.Model;
using Launcher.View.Resources.Script;
using Launcher.View.Resources.Script.Cookie;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Launcher.View.Windows
{
    public partial class SplashScreenWindow : Window
    {
        private DispatcherTimer timer = new DispatcherTimer();

        private bool isAnyUserLoggedIn { get; set; } = false;
        private string loggedInUsername = null;

        public SplashScreenWindow()
        {
            InitializeComponent();
            InitializeFolderAndFile.Initialize();

            Version.Content = $"Версия: {Arguments.curverVersion}";

            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += Timer;
            timer.Start();
        }

        private void Timer(object sender, EventArgs e)
        {
            CheckFolders();
            timer.Stop();
        }

        private void CheckFolders()
        {

            foreach (string folder in Directory.GetDirectories(Paths.userdata, "*", SearchOption.AllDirectories))
            {
                string username = Path.GetFileName(folder);
                string pathConfig = Path.Combine(Paths.userdata, "config.json");

                var data2 = new
                {
                    path = ""
                };

                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                if (!File.Exists(pathConfig)) File.WriteAllText(pathConfig, JsonSerializer.Serialize(data2));

                if (File.Exists(pathConfig)) {
                    var json = File.ReadAllText(pathConfig);
                    var data = JsonSerializer.Deserialize<Config>(json);

                    if (Directory.Exists(folder) && File.Exists(Path.Combine(data?.path, "login.cookie")))
                    {
                        isAnyUserLoggedIn = true;
                        loggedInUsername = username;
                        break;
                    }
                    else
                    {
                        isAnyUserLoggedIn = false;
                        break;
                    }
                }

/*                if (Directory.Exists(folder) && File.Exists(Path.Combine(Paths.cookie(username), "login.cookie")) && ReaderCookie.IsUserLoggedIn(username))
                {
                    isAnyUserLoggedIn = true;
                    loggedInUsername = username;
                    break;
                }
                else
                {
                    isAnyUserLoggedIn = false;
                    break;
                }*/
            }

            if (isAnyUserLoggedIn)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
            else
            {
                AuthorizationWindow authorization = new AuthorizationWindow();
                authorization.Show();
            }

            Close();
        }

        #region dragMove
        private void TopBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        #endregion

        #region WindowsState
        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}
