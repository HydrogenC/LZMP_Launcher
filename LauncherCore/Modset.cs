using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace LauncherCore
{
    public class Modset : IEditable
    {
        public bool HasActualFile
        {
            get => !string.IsNullOrEmpty(filePath);
        }

        public static void ImportFrom(string source)
        {
            File.Copy(source, SharedData.WorkingPath + "\\Sets\\" + Path.GetFileName(source));
        }

        public Modset() { }

        public Modset(string filePath)
        {
            this.filePath = filePath;
            name = Path.GetFileNameWithoutExtension(filePath);
        }

        public Modset(ref Dictionary<string, Mod> dict, string name = "")
        {
            foreach (var i in dict)
            {
                pairs.Add(i.Key, i.Value.ToInstall);
                foreach (var j in i.Value.Addons)
                {
                    pairs.Add(j.Key, j.Value.ToInstall);
                }
            }

            this.name = name;
            loaded = true;
        }

        public Modset(ref Dictionary<string, bool> dict, string name = "")
        {
            pairs = dict;
            this.name = name;
            loaded = true;
        }

        public void Load(bool showInfo = false)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            Unload();

            XmlDocument document = new XmlDocument();
            document.Load(filePath);
            XmlElement root = Scanner.GetElementByTagName(ref document, "settings");
            bool versionConforms = Scanner.GetElementByTagName(ref document, "version").GetAttribute("value") == SharedData.Version;
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
                    pairs.Add(key, bool.Parse(i.GetAttribute("checked")));

                    foreach (XmlElement j in i.ChildNodes)
                    {
                        string addonKey = j.GetAttribute("key");

                        if (SharedData.Mods[key].Addons.ContainsKey(addonKey))
                        {
                            pairs.Add(addonKey, bool.Parse(j.GetAttribute("checked")));
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

            loaded = true;
        }

        public void Unload()
        {
            if (filePath != "")
            {
                pairs = new Dictionary<string, bool>();
            }
        }

        private Dictionary<string, bool> pairs = new Dictionary<string, bool>();
        private bool loaded = false;
        private string filePath = "", name = "";

        /// <summary>
        /// Get the to install state
        /// </summary>
        /// <param name="key">Mod key</param>
        /// <returns>To install state</returns>
        public bool this[string key]
        {
            get
            {
                if (!loaded)
                {
                    Load();
                }

                return pairs[key];
            }
            set
            {
                pairs[key] = value;
            }
        }

        public void Apply()
        {
            if (!loaded)
            {
                Load();
            }

            foreach (var i in pairs)
            {
                Mod target;
                if (Mod.FindMod(ref SharedData.Mods, i.Key, out target))
                {
                    target.ToInstall = i.Value;
                }
            }
        }

        public override void Delete()
        {
            if (filePath != "")
            {

                File.Delete(filePath);
            }
        }

        public override void ExportTo(string dest)
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
                element.SetAttribute("checked", pairs[i.Key].ToString());
                foreach (var j in i.Value.Addons)
                {
                    XmlElement xmlElement = document.CreateElement("mod");
                    xmlElement.IsEmpty = false;
                    xmlElement.SetAttribute("key", j.Key);
                    xmlElement.SetAttribute("checked", pairs[j.Key].ToString());
                    element.AppendChild(xmlElement);
                }
                root.AppendChild(element);
            }
            document.Save(dest);
            filePath = dest;
        }

        public override void Rename(string newName, bool type)
        {
            if (newName == name)
            {
                return;
            }

            string newPath = SharedData.WorkingPath + "\\Sets\\" + newName + ".xml";
            if (HasActualFile)
            {
                File.Move(filePath, newPath);
            }
            else
            {
                ExportTo(newPath);
            }

            name = newName;
            filePath = newPath;
        }

        public override string ToString()
        {
            return name;
        }

        public override string DisplayName
        {
            get => ToString();
        }

        public override string IOFilter
        {
            get => "Xml Modset File（*.xml）|*.xml";
        }
    }
}
