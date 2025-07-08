using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Launcher.View.Pages
{
    public partial class InformationGamePage : Page
    {
        private string _name { get; set; }
        private string _description { get; set; }
        private string _icon { get; set; }
        private string _background { get; set; }

        public InformationGamePage()
        {
            InitializeComponent();
            LoadData();
            Initialize();
        }

        private void LoadData()
        {
            _name = "DefenderRat";
            _icon = "https://raw.githubusercontent.com/RedmerGameAndTechnologies/JsonLauncher/refs/heads/main/icons/DefenderRat/DefenderRat_icon.png";
            _background = "https://raw.githubusercontent.com/RedmerGameAndTechnologies/JsonLauncher/refs/heads/main/background/DefenderRat/DefenderRat_background.png";
        }

        private void Initialize() {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(_background);
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();

            NameGame.Content = $"Купить: {_name}";
            ImageGame.Source = bitmapImage;
        }
    }
}
