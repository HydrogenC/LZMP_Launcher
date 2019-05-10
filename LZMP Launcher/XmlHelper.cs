using System;
using System.Collections.Generic;
using System.Xml;

namespace LZMP_Launcher
{
    class XmlHelper
    {
        private static XmlElement GetElementByTagName(ref XmlElement element, String tagName)
        {
            return (XmlElement)(element.GetElementsByTagName(tagName)[0]);
        }

        private static XmlElement GetElementByTagName(ref XmlDocument element, String tagName)
        {
            return (XmlElement)(element.GetElementsByTagName(tagName)[0]);
        }

        private static void GetModFromElement(XmlElement xml, ref Dictionary<String, Mod> dict, UInt16 category = 0)
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
                Console.WriteLine(i.ToString());
                GetModFromElement(i, ref dict[key].Addons);
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
            GlobalResources.clientLauncher = GlobalResources.workingDir + "\\Client\\" + GetElementByTagName(ref xml, "client").GetAttribute("value");
            GlobalResources.serverLauncher = GlobalResources.workingDir + "\\Server\\" + GetElementByTagName(ref xml, "server").GetAttribute("value");
            GlobalResources.version = GetElementByTagName(ref xml, "version").GetAttribute("value");

            for (UInt16 ct = 0; ct < 3; ct += 1)
            {
                XmlElement node = GetElementByTagName(ref xml, "category-" + ct.ToString());
                foreach (XmlElement i in node.ChildNodes)
                {
                    GetModFromElement(i, ref dict, ct);
                }
            }
        }

        public static void ReadXmlSet(String xmlFile, ref Dictionary<String, Mod> dict)
        {
            XmlDocument document = new XmlDocument();
            document.Load(xmlFile);
            XmlElement root = GetElementByTagName(ref document, "settings");
            foreach (XmlElement i in root.ChildNodes)
            {
                String key = i.GetAttribute("key");
                dict[key].Node.Checked = Boolean.Parse(i.GetAttribute("checked"));
            }
        }

        public static void WriteXmlSet(String xmlFile, ref Dictionary<String, Mod> dict)
        {
            XmlDocument document = new XmlDocument();
            document.CreateXmlDeclaration("1.0", "utf-8", null);
            XmlElement root = document.CreateElement("settings");
            document.AppendChild(root);
            foreach (var i in dict)
            {
                XmlElement element = document.CreateElement("mod");
                root.AppendChild(element);
                element.SetAttribute("key", i.Key);
                element.SetAttribute("checked", i.Value.Node.Checked.ToString());
            }
            document.Save(xmlFile);
        }
    }
}
