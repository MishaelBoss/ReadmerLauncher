using Launcher.View.Components;
using Launcher.View.Resources.Script;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Launcher.View.Windows
{
    public partial class MainWindow : Window
    {
        private Warning userControlWarning = new Warning();
        private WindowState prevState;

        private readonly IHost _host;

        public MainWindow()
        {
            InitializeComponent();
            Initialize();

            _host = Host.CreateDefaultBuilder().ConfigureServices(services => {
                services.AddHostedService<MyBackgroundService>();
            }).Build();

            Task.Run(() => _host.Run());
        }

        private void Initialize() {
            Application.Current.MainWindow = this;

            if (!Internet.connect()) ErrorConnectInternet.Visibility = Visibility.Visible;

            Update.UpdateUI(BackgroundUIFunction, 0, 0, 5);
        }

        private void BackgroundUIFunction(object sender, EventArgs ea) {
            Autoload.SetAutoload();
        }

        private void TopBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void WindowStateChanged(object sender, EventArgs e)
        {
            prevState = WindowState;
        }

        private void TaskbarIconTrayLeftMouseDown(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Minimized) this.WindowState = WindowState.Normal;
            WindowState = prevState;
        }



        /*        protected override async void OnClosed(EventArgs e)
                {
                    await _host.StopAsync();
                    base.OnClosed(e);
                }*/
    }
}