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
        private static XmlElement GetSingleElement(XmlNodeList list)
        {
            return (XmlElement)list[0];
        }

        private static void GetModFromElement(XmlElement xml, Dictionary<String, Mod> dict, UInt16 category = 0)
        {
            if (xml.Name != "mod")
            {
                throw new ArgumentException("Tag name of XmlElement must be 'mod'! ");
            }
            String key = xml.GetAttribute("key");
            dict[key] = new Mod(xml.GetAttribute("name"), category);
            foreach (XmlElement i in xml.GetElementsByTagName("file"))
            {
                dict[key].Files.Add(i.GetAttribute("value"));
            }
            foreach (XmlElement i in xml.GetElementsByTagName("mod"))
            {
                GetModFromElement(i, dict[key].Addons);
            }
        }

        public static void ReadDefinitions(String xmlFile, ref Dictionary<String, Mod> dict)
        {
            if (!System.IO.File.Exists(xmlFile))
            {
                System.Windows.Forms.MessageBox.Show("Settings file not found! ", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }
            XmlDocument xml = new XmlDocument();
            xml.Load(xmlFile);
            GlobalResources.clientLauncher = GlobalResources.workingDir + "\\Client\\" + GetSingleElement(xml.GetElementsByTagName("client")).GetAttribute("value");
            GlobalResources.serverLauncher = GlobalResources.workingDir + "\\Server\\" + GetSingleElement(xml.GetElementsByTagName("server")).GetAttribute("value");
            GlobalResources.version = ((XmlElement)xml.GetElementsByTagName("version")[0]).GetAttribute("value");

            for (UInt16 ct = 0; ct < 3; ct += 1)
            {
                XmlElement node = GetSingleElement(xml.GetElementsByTagName("category-" + ct.ToString()));
                foreach (XmlElement i in node.ChildNodes)
                {
                    GetModFromElement(i, dict, ct);
                }
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
