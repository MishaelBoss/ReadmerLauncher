using System.Windows.Threading;

namespace Launcher.View.Resources.Script
{
    class Update
    {
        private static DispatcherTimer dispatcherTimer;

        public static void UpdateUI(EventHandler BackgroundUIFunction, int hours, int minutes, int seconds)
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(BackgroundUIFunction);
            dispatcherTimer.Interval = new TimeSpan(hours, minutes, seconds);
            dispatcherTimer.Start();
        }
    }
}
