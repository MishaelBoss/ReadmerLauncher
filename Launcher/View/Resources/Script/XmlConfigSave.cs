using LauncherLes1.View.Resources.Script;
using System.IO;
using System.Xml;

namespace Launcher.View.Resources.Script
{
    public class XmlConfigSave
    {
        public static void Change(int elementInt, string elementString, params object[] elementParameters)
        {
            if (Directory.Exists(Paths.config))
            {
                try
                {
                    Save(elementInt, elementString, elementParameters);
                }
                catch (Exception ex)
                {
                    XmlConfigCreate.Create();
                    Save(elementInt, elementString, elementParameters);
                }
            }
            else
            {
                XmlConfigCreate.Create();
            }
        }

        private static void Save(int elementInt, string elementString, params object[] elementParameters)
        {
            string versionFilePath = Path.Combine(Paths.config, "appsettings.xml");
            var doc = new XmlDocument();
            doc.Load(versionFilePath);
            var node = doc.DocumentElement.ChildNodes[elementInt];
            doc.DocumentElement.RemoveChild(node);
            var newNode = doc.CreateElement(elementString);
            newNode.InnerText = string.Join(", ", elementParameters);
            doc.DocumentElement.AppendChild(newNode);
            string realPath = Path.Combine(Paths.config, "appsettings.xml");
            doc.Save(realPath);
        }
    }
}
