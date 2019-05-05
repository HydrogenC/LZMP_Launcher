using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LZMP_Launcher
{
    class XmlHelper
    {
        public static void ReadDefinitions(String xmlFile)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(xmlFile);
            GlobalResources.clientLauncher = GlobalResources.workingDir + "\\Client\\" + xml.GetElementById("client").GetAttribute("file");
            GlobalResources.serverLauncher = GlobalResources.workingDir + "\\Server\\" + xml.GetElementById("server").GetAttribute("file");
            GlobalResources.version = xml.GetElementById("version").GetAttribute("value");
            foreach (var i in xml.GetElementsByTagName("mod"))
            {

            }
        }

        public static void ReadXmlSet(String xmlFile, ref Dictionary<String, Mod> dict)
        {

        }

        public static void WriteXmlSet(String xmlFile, ref Dictionary<String, Mod> dict)
        {

        }
    }
}
