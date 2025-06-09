using System.IO;
using System.Xml.Linq;

namespace Launcher.View.Resources.Script
{
    public class XmlConfigCreate
    {
        public static void Create() {
            if (Directory.Exists(Paths.config))
            {
                createxml();
            }
            else {
                Directory.CreateDirectory(Paths.config);
                createxml();
            }
        }

        private static void createxml() {
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
    }
}
