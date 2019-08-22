using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace LZMP_Launcher
{
    public struct Shared
    {
        public static readonly String workingDir = Directory.GetCurrentDirectory();
        public static readonly String resourceDir = workingDir + "\\Resources\\";
        public static readonly String modDir = workingDir + "\\Game\\.minecraft\\mods\\";
        public static readonly String saveDir = workingDir + "\\Game\\.minecraft\\saves\\";
        public static readonly String scriptDir = workingDir + "\\Game\\.minecraft\\scripts\\";
        public static readonly String jmDataDir = workingDir + "\\Game\\.minecraft\\journeymap\\sp\\";
        public static String launcher, version;
        public static Dictionary<String, Mod> mods = new Dictionary<String, Mod>();
    }

    class HelpFunctions
    {
        private static String GetFileName(String fullPath)
        {
            return fullPath.Substring(fullPath.LastIndexOf('\\') + 1).Replace(".jar", "");
        }

        public static void CleanUp()
        {
            String[] files = Directory.GetFiles(Shared.resourceDir);
            foreach (var i in files)
            {
                Boolean used = false;
                foreach (var j in Shared.mods)
                {
                    foreach (var k in j.Value.Addons)
                    {
                        if (k.Value.Files.Contains(GetFileName(i)))
                        {
                            used = true;
                            break;
                        }
                    }
                    if (j.Value.Files.Contains(GetFileName(i)))
                    {
                        used = true;
                        break;
                    }
                    if (used)
                    {
                        break;
                    }
                }

                if (!used)
                {
                    try
                    {
                        File.Delete(i);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("An error occured while cleaning! ", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    }
                }
            }
        }

        public static void CopyDirectory(String srcPath, String aimPath)
        {
            if (aimPath[aimPath.Length - 1] != '\\')
            {
                aimPath += '\\';
            }
            if (!Directory.Exists(aimPath))
            {
                Directory.CreateDirectory(aimPath);
            }
            String[] fileList = Directory.GetFileSystemEntries(srcPath);
            foreach (string file in fileList)
            {
                if (Directory.Exists(file))
                {
                    CopyDirectory(file, aimPath + Path.GetFileName(file));
                }
                else
                {
                    File.Copy(file, aimPath + Path.GetFileName(file), true);
                }
            }
        }
    }
}
