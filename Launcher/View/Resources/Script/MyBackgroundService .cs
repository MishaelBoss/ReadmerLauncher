using Launcher.View.Windows;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;

namespace Launcher.View.Resources.Script
{
    public class MyBackgroundService : BackgroundService
    {
        private DownloadUpdateLauncherWindow _launcherWindow { get; set; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    _ = Application.Current.Dispatcher.Invoke(async () =>
                    {
                        await CheckForUpdatesAsync(stoppingToken);
                    });

                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                Loges.LoggingProcess(LogLevel.INFO, "Фоновый сервис был отменён");
            }
            catch (Exception ex)
            {
                Loges.LoggingProcess(LogLevel.WARN, "Ошибка в фоновом сервисе", ex: ex);
            }
            finally
            {
                Loges.LoggingProcess(LogLevel.INFO, "Фоновый сервис мониторинга обновлений остановлен");
            }
        }

        private async Task CheckForUpdatesAsync(CancellationToken token)
        {
            try
            {
                await CheckUpdate.CheckUpdateJson();

                if (Arguments.curverVersion.CompareTo(Arguments.readver) < 0 && Arguments.Receive_notifications)
                {
                    if (Arguments.isEmergencyUpdate)
                    {
                        await Application.Current.Dispatcher.InvokeAsync(() =>
                        {
                            try
                            {
                                var notification = new NotificationMessageWindow(
                                    title: "Вышло экстренное обновление!",
                                    message: $"Новая версия {Arguments.readver}\nТекущая версия {Arguments.curverVersion}",
                                    onClick: () =>
                                    {
                                        if (Arguments.aboutVersionLink != null || Arguments.aboutVersionLink != string.Empty) {
                                            try
                                            {
                                                Process.Start(new ProcessStartInfo(Arguments.aboutVersionLink)
                                                {
                                                    UseShellExecute = true
                                                });
                                            }
                                            catch (Exception ex)
                                            {
                                                Loges.LoggingProcess(LogLevel.ERROR, ex: ex);
                                            }
                                        }
                                    });
                            }
                            catch (Exception ex)
                            {
                                Loges.LoggingProcess(LogLevel.ERROR, "Ошибка при показе уведомления", ex: ex);
                            }
                        }, DispatcherPriority.Normal, token);
                    }
                    else {
                        await Application.Current.Dispatcher.InvokeAsync(() =>
                        {
                            try
                            {
                                var notification = new NotificationMessageWindow(
                                    title: "Доступно обновление!",
                                    message: $"Новая версия {Arguments.readver}\nТекущая версия {Arguments.curverVersion}",
                                    onClick: () =>
                                    {
                                        if (Arguments.aboutVersionLink != null || Arguments.aboutVersionLink != string.Empty)
                                        {
                                            try
                                            {
                                                Process.Start(new ProcessStartInfo(Arguments.aboutVersionLink)
                                                {
                                                    UseShellExecute = true
                                                });
                                            }
                                            catch (Exception ex)
                                            {
                                                Loges.LoggingProcess(LogLevel.ERROR, ex: ex);
                                            }
                                        }
                                        else {
                                            Application.Current.MainWindow?.Show();
                                            Application.Current.MainWindow?.Activate();
                                        }
                                    });

                            }
                            catch (Exception ex)
                            {
                                Loges.LoggingProcess(LogLevel.ERROR, "Ошибка при показе уведомления", ex: ex);
                            }
                        }, DispatcherPriority.Normal, token);
                    }
                }

                var _launcherWindow = new DownloadUpdateLauncherWindow();

                if (Arguments.Update_if_is_update) {
                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        _launcherWindow.Show();
                    }, DispatcherPriority.Normal, token);
                }

                if (Arguments.isEmergencyUpdate)
                {
                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        _launcherWindow.Show();
                    }, DispatcherPriority.Normal, token);
                }
            }
            catch (Exception ex)
            {
                Loges.LoggingProcess(LogLevel.ERROR, "Ошибка при проверке обновлений", ex: ex);
            }
        }
    }
}
