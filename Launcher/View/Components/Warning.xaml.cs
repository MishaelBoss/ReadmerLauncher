using System.Windows.Controls;

namespace Launcher.View.Components
{
    public partial class Warning : UserControl
    {
        public Warning()
        {
            InitializeComponent();
        }

        public async void StartWarningAnimation()
        {
            WarningImage.Opacity = 0;
            while (WarningImage.Opacity <= 1)
            {
                WarningImage.Opacity += 0.1;
                await Task.Delay(100);
            }
        }
    }
}
