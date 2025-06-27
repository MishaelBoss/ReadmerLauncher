using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Launcher.View.Components
{
    public partial class ButtonSelectUser : UserControl
    {
        public static readonly DependencyProperty ActionProperty =
            DependencyProperty.Register("Action", typeof(Action), typeof(ButtonSelectUser));

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("SetIcon", typeof(BitmapImage), typeof(ButtonSelectUser), new PropertyMetadata(null));

        public static readonly DependencyProperty UserNameProperty =
            DependencyProperty.Register("SetUserName", typeof(string), typeof(ButtonSelectUser), new PropertyMetadata(string.Empty));

        public Action Action {
            get => (Action)GetValue(ActionProperty);
            set => SetValue(ActionProperty, value);
        }

        public BitmapImage SetIcon
        {
            get => (BitmapImage)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public string SetUserName
        {
            get => (string)GetValue(UserNameProperty);
            set => SetValue(UserNameProperty, value);
        }

        public ButtonSelectUser()
        {
            InitializeComponent();
        }

        private void Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Action?.Invoke();
        }
    }
}
