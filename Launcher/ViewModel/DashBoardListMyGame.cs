using CommunityToolkit.Mvvm.Input;
using Launcher.View.Pages;
using MvvmCross.ViewModels;
using System.Windows.Controls;
using System.Windows.Input;

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
    }
}
