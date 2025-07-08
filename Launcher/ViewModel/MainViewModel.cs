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
        private Page _homePage = new HomePage();
        private Page _myLibaryPage = new MyLibraryPage();
        private Page _settingsPage = new SettingsPage();
        private Page _profilePage = new ProfilePage();
        private Page _downloadListPage = new DownloadListPage();

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
                return new RelayCommand(() => ContentPage = _myLibaryPage);
            }
        }
        public ICommand openSettingsPage
        {
            get
            {
                return new RelayCommand(() => ContentPage = _settingsPage);
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
                return new RelayCommand(() => ContentPage = _profilePage);
            }
        }
        public ICommand openListDownload
        {
            get
            {
                return new RelayCommand(() => ContentPage = _downloadListPage);
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
