using CommunityToolkit.Mvvm.Input;
using Launcher.View.Pages;
using LauncherLes1.Model;
using MvvmCross.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Launcher.ViewModel
{
    public class DashBoardListMyGame : MvxViewModel
    {
        private Page _pageGames = new Game();

        private Page _CurPage;
        public Page ContentPage
        {
            get => _CurPage;
            set => SetProperty(ref _CurPage, value);
        }

        private ObservableCollection<IServer> _serversList = new ObservableCollection<IServer>();
        public ObservableCollection<IServer> ServersList
        {
            get => _serversList;
            set => SetProperty(ref _serversList, value);
        }

        public DashBoardListMyGame()
        {
        }

        #region Переход страниц
        public ICommand openGamesPage
        {
            get
            {
                return new RelayCommand(() => ContentPage = _pageGames);
            }
        }
        #endregion

        private bool _isGameClientLaunching = false;
        public bool IsGameClientLaunching
        {
            get => _isGameClientLaunching;
            set
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(@"C:\Users\GamerVII\Source\Repos\minecraft-launcher\GamerVII.MinecraftLauncher\Views\Resources\Images\default.png");
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                ServersList.Add(new Server
                {
                    Name = "Hitech",
                    //Icon = bitmapImage.ToString()

                });

                ServersList.Add(new Server
                {
                    Name = "Magic",
                    //Icon = bitmapImage.ToString()
                });
            }
        }
    }
}
