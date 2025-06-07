using System.Windows.Controls;

namespace Launcher.View.Components
{
    public partial class ButtonGame : UserControl
    {
        public ButtonGame()
        {
            InitializeComponent();
        }

        public void Add(string name) {
            Name.Content = name;
        }
    }
}
