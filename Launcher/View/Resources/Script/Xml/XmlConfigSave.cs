using System.IO;
using System.Xml.Linq;

namespace Launcher.View.Resources.Script
{
    public class XmlConfigSave
    {
        public static void Change(string elementString, params object[] elementParameters)
        {
            string ConfigFilePath = Path.Combine(Paths.config, Files.configSettingsFileName);

            if (File.Exists(ConfigFilePath))
            {
                Save(elementString, elementParameters);
            }
            else
            {
                XmlConfigCreate.Create();
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
