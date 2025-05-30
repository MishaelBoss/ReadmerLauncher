using CheckConnectInternet;
using Launcher.View.Components;
using Launcher.View.Resources.Script;
using System.Windows;
using System.Windows.Input;

namespace Launcher
{
    public partial class MainWindow : Window
    {
        private ErrorConnectInternet userControl = new ErrorConnectInternet();
        private Warning userControlWarning = new Warning();

        public MainWindow()
        {
            InitializeComponent();
            Initialize();
            Update.UpdateUI(BackgroundUIFunction, 0, 0, 5);
        }

        private void Initialize() {
            if (Internet.connect()) userControl.Visibility = Visibility.Visible;
            else userControl.Visibility = Visibility.Hidden;
        }

        private void BackgroundUIFunction(object sender, EventArgs ea) {
            if (Internet.connect())
            {
                if (ErrorConnectInternet.isIgnoreErrorToConnectInternet) {
                    Warning warning = new Warning();
                    GridMainFrame.Children.Add(warning);
                    warning.StartWarningAnimation();
                }
            }
        }

        private void TopBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}