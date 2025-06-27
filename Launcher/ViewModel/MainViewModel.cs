using CommunityToolkit.Mvvm.Input;
using Launcher.View.Pages;
using Launcher.View.Resources.Script.Cookie;
using Launcher.View.Windows;
using MvvmCross.ViewModels;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Launcher.ViewModel
{
    public class MainViewModel : MvxViewModel
    {
        private Page _homePage = new Home();
        private Page _myGames = new MyLibrary();
        private Page _pageGames = new Game();
        private Page _settings = new Settings();
        private Page _profile = new Profile();

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
        public ICommand openGamesPage
        {
            get
            {
                return new RelayCommand(() => ContentPage = _pageGames);
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
        public ICommand openProfile
        {
            get
            {
                return new RelayCommand(() => ContentPage = _profile);
            }
        }
        public ICommand logout
        {
            get
            {
                return new RelayCommand(() =>
                    Logout()
                );
            }
        }
        #endregion

        private void Logout() {

            LogoutCookie.DeleteCookie("test2");

            Application.Current.MainWindow.Close();
            
            AuthorizationWindow authorizationWindow = new();
            authorizationWindow.Show();
        }
    }
}
