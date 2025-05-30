using GalaSoft.MvvmLight.Command;
using Launcher.View.Pages;
using MvvmCross.ViewModels;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;

namespace Launcher.ViewModel
{
    public class MainViewModel : MvxViewModel
    {
        private Page _homePage = new Home();
        private Page _myGames = new MyGames();
        private Page _settings = new Settings();

        private Page _DashBoard;
        public Page DashBoard
        {
            get => _DashBoard;
            set => SetProperty(ref _DashBoard, value);
        }


        private Page _CurPage;
        public Page ContentPage
        {
            get => _CurPage;
            set => SetProperty(ref _CurPage, value);
        }

        public MainViewModel()
        {
            ContentPage = _homePage;
        }

        #region Переход страниц
        public ICommand openHomePage
        {
            get
            {
                return new RelayCommand(() => ContentPage = _homePage);
            }
        }
        public ICommand openMyGamesPage
        {
            get
            {
                return new RelayCommand(() => ContentPage = _myGames);
            }
        }
        public ICommand openSettingsPage
        {
            get
            {
                return new RelayCommand(() => ContentPage = _settings);
            }
        }
        public ICommand openDiscord
        {
            get
            {
                return new RelayCommand(() => Process.Start(new ProcessStartInfo("https://discord.gg/efEFJfEcXH") { UseShellExecute = true }));
            }
        }
        #endregion
    }
}
