using CommunityToolkit.Mvvm.Input;
using Launcher.View.Pages;
using MvvmCross.ViewModels;
using System.Windows.Controls;
using System.Windows.Input;

namespace Launcher.ViewModel
{
    public class HomeViewModel : MvxViewModel
    {
        private Page _informationGame = new InformationGamePage();

        private Page _CurPage;
        public Page ContentPage
        {
            get => _CurPage;
            set => SetProperty(ref _CurPage, value);
        }

        #region Переход страниц
        public ICommand openInformationGamePage
        {
            get
            {
                return new RelayCommand(() => ContentPage = _informationGame);
            }
        }
        #endregion
    }
}
