using Launcher.View.Resources.Script;
using System.Windows;
using System.Windows.Input;

namespace Launcher.View.Windows
{
    public partial class Authorization : Window
    {
        public Authorization()
        {
            InitializeComponent();
            ShowInTaskbar = true;
            Topmost = true;

            Update.UpdateUI(BackgroundUIFunction, 0, 0, 1);
        }

        private void BackgroundUIFunction(object sender, EventArgs ea) { 
            if(Name.Text != string.Empty && Password.Password != string.Empty) Login.IsEnabled = true;
            else Login.IsEnabled = false;
        }

        #region dragMove
        private void WindowMove(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        #endregion

        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Collapse(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void NotAccount(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Пустышка", "Пустышка");
        }

        private void isRememberClicked(object sender, RoutedEventArgs e)
        {
            var isRemember = Remember.IsChecked ?? false;
        }
    }
}
