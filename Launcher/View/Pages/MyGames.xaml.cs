using Launcher.View.Components;
using System.Windows.Controls;

namespace Launcher.View.Pages
{
    public partial class MyGames : Page
    {
        private Game game = new Game();

        public MyGames()
        {
            InitializeComponent();

            var btn = new ButtonGame()
            {

            };
            LeftBorder.Children.Add(btn);
        }
    }
}