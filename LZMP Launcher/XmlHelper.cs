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

        private static void GetModFromElement(XmlElement xml, ref Dictionary<String, Mod> dict, ModCategory category = ModCategory.Addon)
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

        public static void ReadDefinitions(String xmlFile)
        {
            if (!System.IO.File.Exists(xmlFile))
            {
                System.Windows.Forms.MessageBox.Show("Settings file not found! ", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }

            XmlDocument xml = new XmlDocument();
            xml.Load(xmlFile);
            Shared.clientLauncher = Shared.workingDir + "\\Client\\" + GetElementByTagName(ref xml, "client").GetAttribute("value");
            Shared.serverLauncher = Shared.workingDir + "\\Server\\" + GetElementByTagName(ref xml, "server").GetAttribute("value");
            Shared.version = GetElementByTagName(ref xml, "version").GetAttribute("value");

            foreach (XmlElement element in xml.GetElementsByTagName("category"))
            {
                ModCategory currentCategory;
                switch (element.GetAttribute("name"))
                {
                    case "Technology":
                        currentCategory = ModCategory.Technology;
                        break;
                    case "Warfare":
                        currentCategory = ModCategory.Warfare;
                        break;
                    case "Enhancement":
                        currentCategory = ModCategory.Enhancement;
                        break;
                    default:
                        currentCategory = ModCategory.Addon;
                        break;
                }
                foreach (XmlElement i in element.ChildNodes)
                {
                    GetModFromElement(i, ref Shared.mods, currentCategory);
                }
            }
        }

        public static void ReadXmlSet(String xmlFile)
        {
            XmlDocument document = new XmlDocument();
            document.Load(xmlFile);
            XmlElement root = GetElementByTagName(ref document, "settings");
            foreach (XmlElement i in root.ChildNodes)
            {
                String key = i.GetAttribute("key");
                Shared.mods[key].Node.Checked = Boolean.Parse(i.GetAttribute("checked"));
            }
        }

        public static void WriteXmlSet(String xmlFile)
        {
            XmlDocument document = new XmlDocument();
            document.CreateXmlDeclaration("1.0", "utf-8", null);
            XmlElement root = document.CreateElement("settings");
            document.AppendChild(root);
            foreach (var i in Shared.mods)
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
