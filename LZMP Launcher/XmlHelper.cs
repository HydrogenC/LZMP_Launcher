﻿using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace LZMP_Launcher
{
    class XmlHelper
    {
        private static XmlElement GetElementByTagName(ref XmlDocument element, String tagName)
        {
            return (XmlElement)(element.GetElementsByTagName(tagName)[0]);
        }

        private static Mod ReadMod(XmlElement element)
        {
            Mod mod = new Mod(element.GetAttribute("name"));
            foreach (XmlElement i in element.ChildNodes)
            {
                if (i.Name == "file")
                {
                    mod.Files.Add(i.GetAttribute("value"));
                }
            }
            return mod;
        }

        public static void ReadDefinitions(String xmlFile, ref TreeView view)
        {
            if (!File.Exists(xmlFile))
            {
                MessageBox.Show("Settings file not found! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            XmlDocument xml = new XmlDocument();
            xml.Load(xmlFile);
            Shared.launcher = Shared.workingDir + "\\Game\\" + GetElementByTagName(ref xml, "launcher").GetAttribute("value");
            Shared.version = GetElementByTagName(ref xml, "version").GetAttribute("value");

            foreach (XmlElement element in xml.GetElementsByTagName("category"))
            {
                TreeNode node = new TreeNode(element.GetAttribute("name") + " Mods");
                foreach (XmlElement mod in element.ChildNodes)
                {
                    String key = mod.GetAttribute("key");
                    Shared.mods[key] = ReadMod(mod);
                    foreach (XmlElement addon in mod.ChildNodes)
                    {
                        if (addon.Name == "mod")
                        {
                            String aKey = addon.GetAttribute("key");
                            Shared.mods[key].Addons[aKey] = ReadMod(addon);
                        }
                    }
                    Shared.mods[key].CreateNode();
                    node.Nodes.Add(Shared.mods[key].Node);
                }
                view.Nodes.Add(node);
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
                if (i.Name != "mod")
                {
                    continue;
                }

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
                                MessageBox.Show("Key not found: " + addonKey + " \nSetting file might be broken! ", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
                        MessageBox.Show("Key not found: " + key + " \nSetting file might be broken! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        skip += 1;
                    }
                }
            }

            if (versionConforms)
            {
                MessageBox.Show("Finished! ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Finished! \nSkipped " + skip + " unidentified keys. ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            MessageBox.Show("Finished! ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
