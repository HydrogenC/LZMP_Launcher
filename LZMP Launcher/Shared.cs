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
        public static readonly String jmDataDir = workingDir + "\\Game\\.minecraft\\journeymap\\data\\sp\\";
        public static String launcher, version;
        public static Dictionary<String, Mod> mods = new Dictionary<String, Mod>();
    }

    class Helper
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

            foreach (String file in Directory.GetFileSystemEntries(srcPath))
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

        public static Mod[] GenerateApplyList()
        {
            List<Mod> applyList = new List<Mod>();
            foreach (var i in Shared.mods)
            {
                if ((i.Value.Node.Checked != i.Value.Installed) && i.Value.Available)
                {
                    applyList.Add(i.Value);
                }

                foreach (var j in i.Value.Addons)
                {
                    if ((j.Value.Node.Checked != j.Value.Installed) && j.Value.Available)
                    {
                        applyList.Add(j.Value);
                    }
                }
            }
            return applyList.ToArray();
        }

        public static void CheckInstallation()
        {
            foreach (var i in Shared.mods)
            {
                i.Value.CheckInstalled();
                i.Value.Node.Checked = i.Value.Installed;

                foreach (var j in i.Value.Addons)
                {
                    j.Value.CheckInstalled();
                    j.Value.Node.Checked = j.Value.Installed;
                }
            }
        }
    }
}
