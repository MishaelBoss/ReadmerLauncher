using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Launcher.View.Components
{
    public partial class DashBoard : UserControl
    {
        public static readonly DependencyProperty SetIconUserProperty =
            DependencyProperty.Register("SetIconUser", typeof(BitmapImage), typeof(CartGame), new PropertyMetadata(null));

        public static readonly DependencyProperty SetUserNameProperty =
            DependencyProperty.Register("SetUserName", typeof(string), typeof(CartGame), new PropertyMetadata(string.Empty));

        public BitmapImage SetIconUser
        {
            get => (BitmapImage)GetValue(SetIconUserProperty);
            set => SetValue(SetIconUserProperty, value);
        }

        public string SetUserName
        {
            get => (string)GetValue(SetUserNameProperty);
            set => SetValue(SetUserNameProperty, value);
        }

        public DashBoard()
        {
            InitializeComponent();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }

        private void Collapse(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }
    }
}
