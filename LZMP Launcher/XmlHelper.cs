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

        public static void ReadDefinitions(String xmlFile, ref Dictionary<String, Mod> dict)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(xmlFile);
            GlobalResources.clientLauncher = GlobalResources.workingDir + "\\Client\\" + GetSingleElement(xml.GetElementsByTagName("client")).GetAttribute("file");
            GlobalResources.serverLauncher = GlobalResources.workingDir + "\\Server\\" + GetSingleElement(xml.GetElementsByTagName("server")).GetAttribute("file");
            GlobalResources.version = ((XmlElement)xml.GetElementsByTagName("version")[0]).GetAttribute("value");

            for(UInt16 ct = 0; ct < 3; ct += 1)
            {
                XmlElement node = GetSingleElement(xml.GetElementsByTagName("category-"+ct.ToString()));
                foreach(XmlElement i in node.ChildNodes)
                {
                    String key = i.GetAttribute("key");
                    dict[key] = new Mod(i.GetAttribute("name"), ct, null, null);
                    foreach(XmlElement j in i.ChildNodes)
                    {
                        if (j.Name == "file")
                        {
                            dict[key].Files.Add(j.GetAttribute("file"));
                            continue;
                        }
                        if (j.Name == "addon")
                        {
                            String addonKey = j.GetAttribute("key");
                            dict[key].Addons[addonKey] = new Mod(j.GetAttribute("name"), null);
                            foreach(XmlElement k in j.ChildNodes)
                            {
                                dict[key].Addons[addonKey].Files.Add(k.GetAttribute("file"));
                            }
                        }
                    }
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
