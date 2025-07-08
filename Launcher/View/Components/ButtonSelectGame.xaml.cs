using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Launcher.View.Components
{
    public partial class ButtonSelectGame : UserControl
    {
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(ButtonSelectGame));

        public static readonly DependencyProperty SetIconProperty =
            DependencyProperty.Register("SetIcon", typeof(BitmapImage), typeof(ButtonSelectGame), new PropertyMetadata(null));

        public static readonly DependencyProperty SetNameProperty =
            DependencyProperty.Register("SetName", typeof(string), typeof(ButtonSelectGame), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty SetOpacityNameProperty =
            DependencyProperty.Register("SetOpacityName", typeof(double), typeof(ButtonSelectGame), new PropertyMetadata(null));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public BitmapImage SetIcon
        {
            get => (BitmapImage)GetValue(SetIconProperty);
            set => SetValue(SetIconProperty, value);
        }

        public string SetName
        {
            get => (string)GetValue(SetNameProperty);
            set => SetValue(SetNameProperty, value);
        }

        public double SetOpacityName
        {
            get => (double)GetValue(SetOpacityNameProperty);
            set => SetValue(SetOpacityNameProperty, value);
        }

        public ButtonSelectGame()
        {
            InitializeComponent();
        }
    }
}
