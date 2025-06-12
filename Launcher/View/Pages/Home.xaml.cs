using Launcher.View.Components;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Launcher.View.Pages
{
    public partial class Home : Page
    {
        List<CartGame> btnGame;

        public Home()
        {
            InitializeComponent();
            ConectToBD();
            Load();
        }

        private void ConectToBD()
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(@"https://raw.githubusercontent.com/RedmerGameAndTechnologies/JsonLauncher/refs/heads/main/icons/DefenderRat/Defender%20Rat.png");
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();

            btnGame = new List<CartGame>() {
                new CartGame(){
                    SetIcon = bitmapImage,
                    SetName = "DefenderRat"
                },
                new CartGame(){
                    SetIcon = bitmapImage,
                    SetName = "DefenderRat2"
                }
            };
        }

        private void Load()
        {
            foreach (CartGame button in btnGame)
            {
                Content.Children.Add(button);
            }
        }
    }
}
