using System.Windows.Controls;

namespace Launcher.View.Components
{
    public partial class Warning : UserControl
    {
        public Warning()
        {
            InitializeComponent();
            StartWarningAnimation();
        }

        private async void StartWarningAnimation()
        {
            Warning.Opacity = 0;
            while (Warning.Opacity < 1)
            {
                Warning.Opacity += 0.1;
                await Task.Delay(100);
            }
        }
    }
}
