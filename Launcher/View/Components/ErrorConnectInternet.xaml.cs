using System.Windows;
using System.Windows.Controls;

namespace Launcher.View.Components
{
    public partial class ErrorConnectInternet : UserControl
    {
        public static bool isIgnoreErrorToConnectInternet = false;

        public ErrorConnectInternet()
        {
            InitializeComponent();
        }

        private void ButtonRepeatClick(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonIgnoreClick(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            isIgnoreErrorToConnectInternet = true;
        }
    }
}
