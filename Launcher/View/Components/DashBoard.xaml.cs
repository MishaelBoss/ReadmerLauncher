using System.Windows;
using System.Windows.Controls;

namespace Launcher.View.Components
{
    public partial class DashBoard : UserControl
    {
        public DashBoard()
        {
            InitializeComponent();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow;
            if (mainWindow != null) mainWindow.Close();
        }

        private void Collapse(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow;
            if (mainWindow != null) mainWindow.WindowState = WindowState.Minimized;
        }
    }
}
