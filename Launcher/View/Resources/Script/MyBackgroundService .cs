using Launcher.View.Windows;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;

namespace Launcher.View.Resources.Script
{
    public class MyBackgroundService : BackgroundService
    {
        private readonly ILogger<MyBackgroundService> _logger;
        private DownloadUpdateLauncherWindow _launcherWindow { get; set; }

        public MyBackgroundService(ILogger<MyBackgroundService> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Фоновый сервис мониторинга обновлений запущен");

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    await CheckForUpdatesAsync(stoppingToken);
                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Фоновый сервис был отменён");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка в фоновом сервисе");
            }
            finally
            {
                _logger.LogInformation("Фоновый сервис мониторинга обновлений остановлен");
            }
        }

        private async Task CheckForUpdatesAsync(CancellationToken token)
        {
            try
            {
                _logger.LogInformation("Проверка обновлений...");

                await CheckUpdate.CheckUpdateJson();

                if (Arguments.curverVersion.CompareTo(Arguments.readver) < 0 && Arguments.Receive_notifications)
                {
                    if (Arguments.isEmergencyUpdate)
                    {
                        await Application.Current.Dispatcher.InvokeAsync(() =>
                        {
                            try
                            {
                                var notification = new Notification(
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
                                //notification.ShowNotification(GetNotificationPosition());
                            }
                            catch (Exception ex)
                            {
                                Loges.LoggingProcess(LogLevel.ERROR, ex: ex);
                                _logger.LogError(ex, "Ошибка при показе уведомления");
                            }
                        }, DispatcherPriority.Normal, token);
                    }
                    else {
                        await Application.Current.Dispatcher.InvokeAsync(() =>
                        {
                            try
                            {
                                var notification = new Notification(
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

                                //notification.ShowNotification(GetNotificationPosition());
                            }
                            catch (Exception ex)
                            {
                                Loges.LoggingProcess(LogLevel.ERROR, ex:ex);
                                _logger.LogError(ex, "Ошибка при показе уведомления");
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
                _logger.LogError(ex, "Ошибка при проверке обновлений");
            }
        }
    }
}
