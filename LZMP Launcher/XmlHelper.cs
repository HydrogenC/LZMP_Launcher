using System;
using System.Collections.Generic;
using System.Xml;

namespace LZMP_Launcher
{
    class XmlHelper
    {
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
            foreach (XmlElement i in xml.ChildNodes)
            {
                if (i.Name == "file")
                {
                    dict[key].Files.Add(i.GetAttribute("value"));
                }
            }
            foreach (XmlElement i in xml.GetElementsByTagName("mod"))
            {
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
            Shared.launcher = Shared.workingDir + "\\Client\\" + GetElementByTagName(ref xml, "launcher").GetAttribute("value");
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
            Boolean versionConforms = GetElementByTagName(ref document, "version").GetAttribute("value") == Shared.version;
            UInt16 skip = 0;
            foreach (XmlElement i in root.ChildNodes)
            {
                String key = i.GetAttribute("key");

                if (Shared.mods.ContainsKey(key))
                {
                    Shared.mods[key].Node.Checked = Boolean.Parse(i.GetAttribute("checked"));

                    foreach (XmlElement j in i.ChildNodes)
                    {
                        String addonKey = j.GetAttribute("key");

                        if (Shared.mods[key].Addons.ContainsKey(addonKey))
                        {
                            Shared.mods[key].Addons[addonKey].Node.Checked = Boolean.Parse(j.GetAttribute("checked"));
                        }
                        else
                        {
                            if (versionConforms)
                            {
                                System.Windows.Forms.MessageBox.Show("Key not found: " + addonKey + " \nSetting file might be broken! ", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                            }
                            else
                            {
                                skip += 1;
                            }
                        }
                    }
                }
                else
                {
                    if (versionConforms)
                    {
                        System.Windows.Forms.MessageBox.Show("Key not found: " + key + " \nSetting file might be broken! ", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    }
                    else
                    {
                        skip += 1;
                    }
                }
            }

            if (versionConforms)
            {
                System.Windows.Forms.MessageBox.Show("Finished! ", "Information", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Finished! \nSkipped " + skip + " unidentified keys. ", "Information", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }
        }

        public static void WriteXmlSet(String xmlFile)
        {
            XmlDocument document = new XmlDocument();
            document.CreateXmlDeclaration("1.0", "utf-8", null);
            XmlElement root = document.CreateElement("settings");
            document.AppendChild(root);
            XmlElement ver = document.CreateElement("version");
            ver.SetAttribute("value", Shared.version);
            root.AppendChild(ver);
            foreach (var i in Shared.mods)
            {
                XmlElement element = document.CreateElement("mod");
                element.IsEmpty = false;
                element.SetAttribute("key", i.Key);
                element.SetAttribute("checked", i.Value.Node.Checked.ToString());
                foreach (var j in i.Value.Addons)
                {
                    XmlElement xmlElement = document.CreateElement("mod");
                    xmlElement.IsEmpty = false;
                    xmlElement.SetAttribute("key", j.Key);
                    xmlElement.SetAttribute("checked", j.Value.Node.Checked.ToString());
                    element.AppendChild(xmlElement);
                }
                root.AppendChild(element);
            }
            document.Save(xmlFile);

            System.Windows.Forms.MessageBox.Show("Finished! ", "Information", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
        }
    }
}
