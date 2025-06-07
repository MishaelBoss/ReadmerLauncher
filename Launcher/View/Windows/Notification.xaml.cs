using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Launcher.View.Windows
{
    public partial class Notification : Window
    {
        public Action ClickAction { get; set; }

        public Notification()
        {
            InitializeComponent();
            Loaded += CustomNotification_Loaded;
            ShowInTaskbar = false;
            Topmost = true;
        }

        public Notification(string title, string message, BitmapImage image = null, Action onClick = null) : this()
        {
            TitleText.Text = title;
            ContentText.Text = message;
            if (image != null)
            {
                Icon.Source = image;
                Icon.Visibility = Visibility.Visible;
            }
            if (onClick != null) LinkText.Visibility = Visibility.Visible;
            else LinkText.Visibility = Visibility.Hidden;
            ClickAction = onClick;
        }

        private void CustomNotification_Loaded(object sender, RoutedEventArgs e)
        {
            MouseDown += (s, args) =>
            {
                ClickAction?.Invoke();
                Close();
            };

            // Автоматическое закрытие уведомления через 5 секунд
            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5) };
            timer.Tick += (s, args) =>
            {
                timer.Stop();
                Close();
            };
            timer.Start();
        }

        public void ShowNotification(Point location)
        {
            Left = location.X;
            Top = location.Y;
            Show();
        }
    }
}
