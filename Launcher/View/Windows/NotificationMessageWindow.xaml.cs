using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Launcher.View.Windows
{
    public partial class NotificationMessageWindow : Window
    {
        public Action ClickAction { get; set; }

        public NotificationMessageWindow()
        {
            InitializeComponent();
            Loaded += CustomNotification_Loaded;
            ShowInTaskbar = false;
            Topmost = true;

            ShowNotification(GetNotificationPosition());
        }

        public NotificationMessageWindow(string title, string message, BitmapImage image = null, Action onClick = null) : this()
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
            MouseLeftButtonUp += (s, args) =>
            {
                ClickAction?.Invoke();
                Close();
            };

            MouseRightButtonDown += (s, args) =>
            {
                Close();
            };

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

        private Point GetNotificationPosition()
        {
            double x = SystemParameters.WorkArea.Right - 320;
            double y = SystemParameters.WorkArea.Bottom - 200;
            return new Point(x, y);
        }
    }
}
