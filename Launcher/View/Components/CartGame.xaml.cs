using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Launcher.View.Components
{
    public partial class CartGame : UserControl
    {
        public CartGame()
        {
            InitializeComponent();
        }

        public BitmapImage SetIcon
        {
            get => (BitmapImage)Image.Source;
            set => Image.Source = value;
        }

        public string SetName
        {
            get => (string)Name.Content;
            set => Name.Content = value;
        }
    }
}
