using LauncherLes1.View.Resources.Script;
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
                    string versionFilePath = Path.Combine(Paths.config, "appsettings.xml");
                    XDocument xdoc = XDocument.Load(versionFilePath);

                    bool isUpdate = bool.TryParse(xdoc.Element("protocol")?.Element("update_if_is_update")?.Value, out var result) && result;
                    bool isAutoload = bool.TryParse(xdoc.Element("protocol")?.Element("autoload")?.Value, out var result2) && result2;
                    bool isNotifications = bool.TryParse(xdoc.Element("protocol")?.Element("receive_notifications")?.Value, out var result3) && result3;

                    Arguments.Update_if_is_update = isUpdate;
                    Arguments.Autoload = isAutoload;
                    Arguments.Receive_notifications = isNotifications;

/*
                    XElement? protocol = xdoc.Element("protocol");
                    if (protocol != null)
                    {
                        XElement? update_if_is_update = protocol.Element("update_if_is_update");
                        if (update_if_is_update != null) Arguments.CheckBox_update_if_is_update = ((bool)update_if_is_update);
                    }*/
                }
                catch (Exception e)
                {
                    //Loges.LoggingProcess("EXCEPTION E: " + e.Message);
                }
            }
            //else Loges.LoggingProcess("Директория не найдена: " + Paths.config);
        }
    }
}
