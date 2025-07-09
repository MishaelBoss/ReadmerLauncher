using System.IO;
using System.Xml.Linq;

namespace Launcher.View.Resources.Script.Xml
{
    public class ManagerSettingsXML
    {
        public static void Create()
        {
            if(!Directory.Exists(Paths.config)) Directory.CreateDirectory(Paths.config);

            createxml();
        }

        private static void createxml()
        {
            int trackId = 1;
            XDocument xdoc = new XDocument(
                new XElement("protocol",
                new XElement("update_if_is_update", $"{false}"),
                new XElement("autoload", $"{true}"),
                new XElement("receive_notifications", $"{true}"),
                new XElement("speed",
                    new XElement("update", $"{999999}", new XAttribute("id", trackId++)),
                        new XElement("game", $"{999999}", new XAttribute("id", trackId++)))));

            string realPath = Path.Combine(Paths.config, Files.configSettingsFileName);
            xdoc.Save(realPath);
        }

        public static void Load()
        {
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

        public static void Change(string elementString, params object[] elementParameters)
        {
            string ConfigFilePath = Path.Combine(Paths.config, Files.configSettingsFileName);

            if (File.Exists(ConfigFilePath))
            {
                Save(elementString, elementParameters);
            }
            else
            {
                ManagerSettingsXML.Create();
            }
        }

        private static void Save(string elementString, params object[] elementParameters)
        {
            try
            {
                string ConfigFilePath = Path.Combine(Paths.config, Files.configSettingsFileName);

                var xdoc = File.Exists(ConfigFilePath)
                    ? XDocument.Load(ConfigFilePath)
                    : new XDocument(new XElement("protocol"));

                var pathParts = elementString.Split('/');
                XElement currentElement = xdoc.Root;

                for (int i = 0; i < pathParts.Length - 1; i++)
                {
                    var nextElement = currentElement.Element(pathParts[i]);
                    if (nextElement == null)
                    {
                        nextElement = new XElement(pathParts[i]);
                        currentElement.Add(nextElement);
                    }
                    currentElement = nextElement;
                }

                var targetElement = currentElement.Element(pathParts.Last());
                string value = string.Join(", ", elementParameters);

                targetElement.Value = value;

                xdoc.Save(ConfigFilePath);
            }
            catch (Exception ex)
            {
                Loges.LoggingProcess(LogLevel.ERROR, $"Failed to save XML element: {elementString}", ex);
            }
        }
    }
}
