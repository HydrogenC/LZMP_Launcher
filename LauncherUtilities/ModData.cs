using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tomlyn;
using Tomlyn.Model;

namespace LauncherUtilities
{
    class ModData
    {
        public static string TempFolder = Path.GetTempPath();

        public string Version { get; set; }
        public string DisplayName { get; set; }
        public string ModId { get; set; }
        public string DisplayUrl { get; set; }

        public ModData(string jarPath)
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);

            FastZip fastZip = new FastZip();
            fastZip.ExtractZip(jarPath, tempDirectory, @"+\.toml$;+\.MF$");

            string tomlPath = Path.Combine(tempDirectory, "META-INF\\mods.toml");
            if (!File.Exists(tomlPath))
            {
                throw new FileNotFoundException("No mods.toml found! ");
            }

            var tomlModel = Toml.ToModel(File.ReadAllText(tomlPath));
            var modsTable = (TomlTableArray)tomlModel["mods"]!;

            DisplayName = GetValueOrNull(modsTable[0], "displayName");
            Version = GetValueOrNull(modsTable[0], "version");
            ModId = GetValueOrNull(modsTable[0], "modId");
            DisplayUrl = GetValueOrNull(modsTable[0], "displayURL");

            if (Version == "${file.jarVersion}")
            {
                string manifestPath = Path.Combine(tempDirectory, "META-INF\\MANIFEST.MF");
                foreach (var item in File.ReadAllLines(manifestPath))
                {
                    string[] parts = item.Split(':');
                    if (parts[0].Trim() == "Implementation-Version")
                    {
                        Version = parts[1].Trim();
                        break;
                    }
                }
            }
        }

        static string GetValueOrNull(TomlTable table, string tomlKey)
        {
            if (table.ContainsKey(tomlKey))
            {
                return (string)table[tomlKey];
            }
            else
            {
                return null;
            }
        }
    }
}
