using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LauncherCore
{
    public static class Scanner
    {

        public static EditableObject[] ScanForMaps()
        {
            List<EditableObject> saves = new List<EditableObject>();
            if (!Directory.Exists(SharedData.SavePath))
            {
                Directory.CreateDirectory(SharedData.SavePath);
            }

            foreach (string i in Directory.GetDirectories(SharedData.SavePath))
            {
                if (File.Exists(i + "\\level.dat"))
                {
                    try
                    {
                        saves.Add(new Save(i));
                    }
                    catch (Exception) { }
                }
            }
            return saves.ToArray();
        }

        public static EditableObject[] ScanForModsets()
        {
            if (!Directory.Exists(SharedData.WorkingPath + "\\Sets"))
            {
                Directory.CreateDirectory(SharedData.WorkingPath + "\\Sets");
            }

            List<EditableObject> modsets = new List<EditableObject>();
            modsets.Add(new Modset(ref SharedData.Mods, "(Current)"));
            foreach (string i in Directory.EnumerateFiles(SharedData.WorkingPath + "\\Sets"))
            {
                try
                {
                    modsets.Add(new Modset(i));
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return modsets.ToArray();
        }

        public static (string, string)[] ScanCurseforgeLinks(string xmlFile)
        {
            List<(string, string)> lst = new List<(string, string)>();
            XmlDocument xml = new XmlDocument();
            xml.Load(xmlFile);
            foreach (XmlElement i in xml.GetElementsByTagName("file"))
            {
                lst.Add((i.GetAttribute("value"), i.GetAttribute("cfg")));
            }
            return lst.ToArray();
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

        public static XmlElement GetElementByTagName(ref XmlDocument element, string tagName)
        {
            return (XmlElement)(element.GetElementsByTagName(tagName)[0]);
        }

        public static void ScanConfig(string xmlFile)
        {
            if (!File.Exists(xmlFile))
            {
                SharedData.DisplayMessage("Settings file not found! ", "Error", MessageType.Error);
                return;
            }

            XmlDocument xml = new XmlDocument();
            xml.Load(xmlFile);
            XmlElement packElement = GetElementByTagName(ref xml, "pack");
            SharedData.LauncherPath = SharedData.WorkingPath + "\\" + packElement.GetAttribute("launcher");
            SharedData.Version = packElement.GetAttribute("version");
            SharedData.Title = packElement.GetAttribute("title").Replace("%v", SharedData.Version);

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
    }
}
