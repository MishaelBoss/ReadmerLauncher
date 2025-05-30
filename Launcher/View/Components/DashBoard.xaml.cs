using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
