using Launcher.View.Pages;
using Launcher.View.Resources.Script;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Launcher.View.Windows
{
    public partial class AuthorizationWindow : Window
    {
        private bool isFolderUser { get; set; } = false;

        public static NavigationService navigationService;

        public AuthorizationWindow()
        {
            InitializeComponent();

            navigationService = MainFrame.NavigationService;

            if (Directory.Exists(Paths.userdata))
            {
                foreach (string folder in Directory.GetDirectories(Paths.userdata, "*", SearchOption.AllDirectories))
                {
                    if (Directory.Exists(folder))
                    {
                        isFolderUser = true;
                        break;
                    }
                    else
                    {
                        isFolderUser = false;
                        break;
                    }
                }
            }
            else 
            {
                Directory.CreateDirectory(Paths.userdata);
                return;
            }

            if (isFolderUser)
            {
                SelectUser selectUser = new();
                navigationService.Navigate(selectUser);
            }
            else {
                Authorization authorization = new();
                navigationService.Navigate(authorization);
            }
        }

        #region dragMove
        private void WindowMove(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        #endregion

        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Collapse(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
