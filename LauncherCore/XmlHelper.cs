using System;
using System.IO;
using System.Xml;

namespace LauncherCore
{
    public class XmlHelper
    {
        private static XmlElement GetElementByTagName(ref XmlDocument element, string tagName)
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

        public static void ReadDefinitions(string xmlFile)
        {
            if (!File.Exists(xmlFile))
            {
                SharedData.DisplayMessage("Settings file not found! ", "Error", MessageType.Error);
                return;
            }

            XmlDocument xml = new XmlDocument();
            xml.Load(xmlFile);
            SharedData.LauncherPath = SharedData.WorkingPath + "\\" + GetElementByTagName(ref xml, "pack").GetAttribute("launcher");
            SharedData.Version = GetElementByTagName(ref xml, "pack").GetAttribute("version");
            SharedData.Title = GetElementByTagName(ref xml, "pack").GetAttribute("title").Replace("%v", SharedData.Version);

            foreach (XmlElement element in xml.GetElementsByTagName("category"))
            {
                string ct = element.GetAttribute("name");

                foreach (XmlElement mod in element.ChildNodes)
                {
                    string key = mod.GetAttribute("key");
                    SharedData.Mods[key] = ReadMod(mod);
                    SharedData.Mods[key].Category = ct;
                    SharedData.Mods[key].Key = key;

                    foreach (XmlElement addon in mod.ChildNodes)
                    {
                        if (addon.Name == "mod")
                        {
                            string aKey = addon.GetAttribute("key");
                            SharedData.Mods[key].Addons[aKey] = ReadMod(addon);
                            SharedData.Mods[key].Addons[aKey].Key = aKey;
                        }
                    }
                }
            }
        }

        public static void ReadXmlSet(string xmlFile, bool showInfo = true)
        {
            XmlDocument document = new XmlDocument();
            document.Load(xmlFile);
            XmlElement root = GetElementByTagName(ref document, "settings");
            bool versionConforms = GetElementByTagName(ref document, "version").GetAttribute("value") == SharedData.Version;
            ushort skip = 0;

            foreach (XmlElement i in root.ChildNodes)
            {
                if (i.Name != "mod")
                {
                    continue;
                }

                string key = i.GetAttribute("key");

                if (SharedData.Mods.ContainsKey(key))
                {
                    SharedData.Mods[key].ToInstall = bool.Parse(i.GetAttribute("checked"));

                    foreach (XmlElement j in i.ChildNodes)
                    {
                        string addonKey = j.GetAttribute("key");

                        if (SharedData.Mods[key].Addons.ContainsKey(addonKey))
                        {
                            SharedData.Mods[key].Addons[addonKey].ToInstall = bool.Parse(j.GetAttribute("checked"));
                        }
                        else
                        {
                            if (versionConforms)
                            {
                                SharedData.DisplayMessage("Key not found: " + addonKey + " \nSetting file might be broken! ", "Error", MessageType.Error);
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
                        SharedData.DisplayMessage("Key not found: " + key + " \nSetting file might be broken! ", "Error", MessageType.Error);
                    }
                    else
                    {
                        skip += 1;
                    }
                }
            }

            if (showInfo)
            {
                if (versionConforms)
                {
                    SharedData.DisplayMessage("Finished! ", "Information", MessageType.Info);
                }
                else
                {
                    SharedData.DisplayMessage("Finished! \nSkipped " + skip + " unidentified keys. ", "Information", MessageType.Info);
                }
            }
        }

        public static void WriteXmlSet(string xmlFile, bool showInfo = true)
        {
            XmlDocument document = new XmlDocument();
            document.CreateXmlDeclaration("1.0", "utf-8", null);
            XmlElement root = document.CreateElement("settings");
            document.AppendChild(root);
            XmlElement ver = document.CreateElement("version");
            ver.SetAttribute("value", SharedData.Version);
            root.AppendChild(ver);
            foreach (var i in SharedData.Mods)
            {
                XmlElement element = document.CreateElement("mod");
                element.IsEmpty = false;
                element.SetAttribute("key", i.Key);
                element.SetAttribute("checked", i.Value.ToInstall.ToString());
                foreach (var j in i.Value.Addons)
                {
                    XmlElement xmlElement = document.CreateElement("mod");
                    xmlElement.IsEmpty = false;
                    xmlElement.SetAttribute("key", j.Key);
                    xmlElement.SetAttribute("checked", j.Value.ToInstall.ToString());
                    element.AppendChild(xmlElement);
                }
                root.AppendChild(element);
            }
            document.Save(xmlFile);

            if (showInfo)
            {
                SharedData.DisplayMessage("Finished! ", "Information", MessageType.Info);
            }
        }
    }
}
