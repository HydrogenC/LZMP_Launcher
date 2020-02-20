using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LauncherCore
{
    public class Scanner
    {

        public static Save[] ScanForMaps()
        {
            List<Save> saves = new List<Save>();
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

        public static Modset[] ScanForModsets()
        {
            List<Modset> modsets = new List<Modset>();
            modsets.Add(new Modset(ref SharedData.Mods));
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
    }
}
