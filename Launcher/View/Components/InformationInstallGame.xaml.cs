using Launcher.View.Resources.Script;
using Launcher.View.Resources.Script.Json;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Launcher.View.Components
{
    public partial class InformationInstallGame : UserControl
    {
        public static InformationInstallGame Instance { get; private set; }

        public double id { get; set; }
        private string pathInstallation { get; set; }
        private bool isDesktop { get; set; }
        private bool isStartMenu { get; set; }

        public string SetName
        {
            get => (string)NameGame.Content;
            set => NameGame.Content = value;
        }

        public BitmapImage SetIcon
        {
            get => (BitmapImage)ImageGame.Source;
            set => ImageGame.Source = value;
        }

        public string SetSize
        {
            get => (string)SizeGame.Content;
            set => SizeGame.Content = value;
        }

        public InformationInstallGame()
        {
            InitializeComponent();
            Instance = this;
            BtnInstall.IsEnabled = false;
        }

        private void CheckBoxCreateDesktopChecked(object sender, RoutedEventArgs e)
        {
            isDesktop = true;
        }

        private void CheckBoxCreateDesktopUnchecked(object sender, RoutedEventArgs e)
        {
            isDesktop = false;
        }

        private void CheckBoxStartMenuChecked(object sender, RoutedEventArgs e)
        {
            isStartMenu = true;
        }

        private void CheckBoxStartMenuUnchecked(object sender, RoutedEventArgs e)
        {
            isStartMenu = false;
        }

        private void ViewInstallationPath(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog openFolderDialog = new OpenFolderDialog();
            if (openFolderDialog.ShowDialog() == true) {
                pathInstallation = openFolderDialog.FolderName;
                PathIntallation.Content = pathInstallation;
                BtnInstall.IsEnabled = true;
            }
        }

        private void Install(object sender, RoutedEventArgs e)
        {
            if (pathInstallation != null)
            {
                string LibraryfoldersFilePath = Path.Combine(Paths.work, Files.LibraryfoldersJson);

                if(!File.Exists(LibraryfoldersFilePath)) JsonConfidCreate.CreateLibraryFolders();

                try
                {
                    JsonConfidCreate.Create(id,SetName, pathInstallation);
                    if (isDesktop) CreateIcon.CreateShortcut(ShortcutLocation.DESKTOP, SetName);
                    else if (isStartMenu) CreateIcon.CreateShortcut(ShortcutLocation.START_MENU, SetName);
                    else if (isDesktop && isStartMenu) CreateIcon.CreateShortcut(ShortcutLocation.All, SetName);
                    Visibility = Visibility.Collapsed;
                }
                catch (Exception ex)
                {
                    Loges.LoggingProcess(level: LogLevel.WARN, ex: ex);
                }
            }
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void Close(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
    }
}
