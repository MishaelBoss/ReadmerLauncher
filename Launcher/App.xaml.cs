using Launcher.View.Resources.Script;
using System.Windows;
using System.Windows.Threading;

namespace Launcher
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);

            AppDomain.CurrentDomain.UnhandledException += Loges.ExceptionEventApp;
            DispatcherUnhandledException += AppDispatcherUnhandledException;
        }

        private void AppDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Loges.LoggingProcess(
                level: LogLevel.ERROR, 
                ex: e.Exception
            );
            e.Handled = true;
        }
    }
}