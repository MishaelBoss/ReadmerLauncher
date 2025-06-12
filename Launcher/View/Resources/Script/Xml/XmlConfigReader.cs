using System.IO;
using System.Xml.Linq;

namespace Launcher.View.Resources.Script
{
    public class XmlConfigReader
    {
        public static void Load() {
            if (Directory.Exists(Paths.config))
            {
                try
                {
                    string versionFilePath = Path.Combine(Paths.config, Files.configSettingsFileName);
                    XDocument xdoc = XDocument.Load(versionFilePath);

                    XElement protocol = xdoc.Element("protocol");
                    if (protocol != null)
                    {
                        XElement isUpdate = protocol.Element("update_if_is_update");
                        XElement isAutoload = protocol.Element("autoload");
                        XElement isNotifications = protocol.Element("receive_notifications");

                        Arguments.Update_if_is_update = (bool)isUpdate;
                        Arguments.Autoload = (bool)isAutoload;
                        Arguments.Receive_notifications = (bool)isNotifications;

                        XElement speed = xdoc.Element("speed");
                        if (speed != null)
                        {
                            XElement SpeedDownloadUpdate = protocol.Element("update");
                            XElement SpeedDownloadGame = protocol.Element("game");

                            if (SpeedDownloadUpdate != null) Arguments.Speed_download_update = (int)SpeedDownloadUpdate;
                            if (SpeedDownloadGame != null) Arguments.Speed_download_game = (int)SpeedDownloadGame;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Loges.LoggingProcess(LogLevel.ERROR, ex: ex);
                }
            }
            else Loges.LoggingProcess(LogLevel.ERROR, "Директория не найдена: " + Paths.config);
        }
    }
}
