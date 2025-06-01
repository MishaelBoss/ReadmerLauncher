using LauncherLes1.View.Resources.Script;
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
            XDocument xdoc = new XDocument(new XElement("protocol",
                new XElement("update_if_is_update", $"{false}"),
                new XElement("autoload", $"{true}"),
                new XElement("receive_notifications", $"{true}")));
            string realPath = Path.Combine(Paths.config, "appsettings.xml");
            xdoc.Save(realPath);
        }
    }
}
