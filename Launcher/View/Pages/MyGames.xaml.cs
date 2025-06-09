using Launcher.View.Components;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Launcher.View.Pages
{
    public partial class MyGames : Page
    {
        List<ButtonGame> btnGame;

        public MyGames()
        {
            InitializeComponent();
            ConectToBD();
            Load();
        }

        private void ConectToBD() {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(@"https://raw.githubusercontent.com/RedmerGameAndTechnologies/JsonLauncher/refs/heads/main/icons/DefenderRat/Defender%20Rat.png");
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();

            btnGame = new List<ButtonGame>() {
                new ButtonGame(){
                    SetIcon = bitmapImage,
                    SetName = "DefenderRat"
                },
                new ButtonGame(){
                    SetIcon = bitmapImage,
                    SetName = "DefenderRat2"
                }
            };
        }

        private void Load() {
            foreach (ButtonGame button in btnGame) {
                LeftBorder.Children.Add(button);
            }
        }
    }
}