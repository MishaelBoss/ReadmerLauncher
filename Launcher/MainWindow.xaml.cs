using CheckConnectInternet;
using Launcher.View.Components;
using Launcher.View.Resources.Script;
using System.Windows;
using System.Windows.Input;

namespace Launcher
{
    public partial class MainWindow : Window
    {
        private ErrorConnectInternet _userControl;

        public MainWindow()
        {
            InitializeComponent();
            Initialize();
            Update.UpdateUI(BackgroundUIFunction, 0, 0, 5);
        }

        private void Initialize() {
/*            if (Internet.connect()) {
                ErrorConnectInternet userControl = new ErrorConnectInternet();
                _userControl.Visibility = Visibility.Visible;
            }*/
        }

        private void BackgroundUIFunction(object sender, EventArgs ea) {
            if (Internet.connect())
            {
                StartWarningAnimation();
            }
            else Warning.Opacity = 0;
        }

        private async void StartWarningAnimation()
        {
            Warning.Opacity = 0;
            while (Warning.Opacity < 1)
            {
                Warning.Opacity += 0.1;
                await Task.Delay(100);
            }
        }

        private void TopBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}