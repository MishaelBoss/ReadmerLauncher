using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Launcher.View.Components
{
    public partial class ButtonGame : UserControl
    {
        public ButtonGame()
        {
            InitializeComponent();
        }

        public BitmapImage SetIcon
        {
            get => (BitmapImage)Icon.Source;
            set => Icon.Source = value;
        }

        public string SetName {
            get => (string)Name.Content;
            set => Name.Content = value;
        }
    }
}
