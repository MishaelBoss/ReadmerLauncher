using System.IO;
using System.Net.Http;
using System.Windows;

namespace Launcher.View.Resources.Script.Game
{
    public class DownloadContent
    {
        public static void Download(string destinationPath, string url)
        {
            try
            {
                if (Internet.connect())
                {
                    using (var client = new HttpClient())
                    {
                        try
                        {
                            using (var s = client.GetStreamAsync(url))
                            {
                                using (var fs = new FileStream(destinationPath, FileMode.OpenOrCreate))
                                {
                                    s.Result.CopyTo(fs);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Loges.LoggingProcess(LogLevel.ERROR, ex: ex);
                            MessageBox.Show($"Ошибка при загрузке контента: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else
                {
                    Loges.LoggingProcess(LogLevel.INFO, "Подключитесь к интернету");
                    MessageBox.Show("Вовремя работы отключился интернет", "Подключитесь к интернету", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                Loges.LoggingProcess(LogLevel.INFO, ex: ex);
            }
        }
    }
}
