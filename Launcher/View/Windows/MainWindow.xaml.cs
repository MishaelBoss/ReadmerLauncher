﻿using Launcher.View.Components;
using Launcher.View.Resources.Script;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using System.Windows.Input;

namespace Launcher.View.Windows
{
    public partial class MainWindow : Window
    {
        private Warning userControlWarning = new Warning();

        private Authorization Authorization = new Authorization();

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
            InitializeFolderAndFile.Initialize();

            if (!Internet.connect()) ErrorConnectInternet.Visibility = Visibility.Visible;

            Update.UpdateUI(BackgroundUIFunction, 0, 0, 5);

            Authorization.Show();
            this.Close();
        }

        private void BackgroundUIFunction(object sender, EventArgs ea) {
/*            if (!Internet.connect())
            {
                if (ErrorConnectInternet.isIgnoreErrorToConnectInternet)
                {
                    if (!GridMainFrame.Children.Contains(userControlWarning)) {
                        GridMainFrame.Children.Add(userControlWarning);
                        userControlWarning.StartWarningAnimation();
                    }
                }
            }
            else if (GridMainFrame.Children.Contains(userControlWarning)) GridMainFrame.Children.Remove(userControlWarning);*/

            Autoload.SetAutoload();
        }

        private void TopBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        

        /*        protected override async void OnClosed(EventArgs e)
                {
                    await _host.StopAsync();
                    base.OnClosed(e);
                }*/
    }
}