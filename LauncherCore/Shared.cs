using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace LauncherCore
{
    public struct Shared
    {
        public static String WorkingDir = Directory.GetCurrentDirectory();

        public static String ResourceDir
        {
            get => WorkingDir + "\\Resources\\";
        }

        public static String ModDir
        {
            get => WorkingDir + "\\Game\\.minecraft\\mods\\";
        }

        public static String SaveDir
        {
            get => WorkingDir + "\\Game\\.minecraft\\saves\\";
        }

        public static String ScriptDir
        {
            get => WorkingDir + "\\Game\\.minecraft\\scripts\\";
        }

        public static String JMDataDir
        {
            get => WorkingDir + "\\Game\\.minecraft\\journeymap\\data\\sp\\";
        }

        public static String LauncherPath, Version;
        public static Dictionary<String, Mod> Mods = new Dictionary<String, Mod>();
    }

    public class LCore
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="check">If true, then check all mods. If false, cancel all mods. </param>
        public static void CheckAll(Boolean check = true)
        {
            foreach (var i in Shared.Mods)
            {
                if (i.Value.ToInstall != check)
                {
                    i.Value.ToInstall = check;
                }

                foreach (var j in i.Value.Addons)
                {
                    if (j.Value.ToInstall != check)
                    {
                        i.Value.ToInstall = check;
                    }
                }
            }
        }

        public static void ApplyChanges(ref Int32 total, ref Int32 current)
        {
            List<Mod> applyList = new List<Mod>();
            foreach (var i in Shared.Mods)
            {
                if ((i.Value.ToInstall != i.Value.Installed) && i.Value.Available)
                {
                    applyList.Add(i.Value);
                }

                foreach (var j in i.Value.Addons)
                {
                    if ((j.Value.ToInstall != j.Value.Installed) && j.Value.Available)
                    {
                        applyList.Add(j.Value);
                    }
                }
            }

            if (applyList.Count == 0)
            {
                return;
            }

            total = applyList.Count;
            current = 1;

            foreach (var i in applyList)
            {

                if (i.Installed)
                {
                    i.Uninstall();
                }
                else
                {
                    i.Install();
                }
                current += 1;
            }

            LCore.CheckInstallation();
        }

        private static String GetFileName(String fullPath)
        {
            return fullPath.Substring(fullPath.LastIndexOf('\\') + 1).Replace(".jar", "");
        }

        public static void CleanUp()
        {
            String[] files = Directory.GetFiles(Shared.ResourceDir);
            foreach (var i in files)
            {
                Boolean used = false;
                foreach (var j in Shared.Mods)
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

        public static void CheckInstallation()
        {
            foreach (var i in Shared.Mods)
            {
                i.Value.CheckInstalled();
                i.Value.ToInstall = i.Value.Installed;

                foreach (var j in i.Value.Addons)
                {
                    j.Value.CheckInstalled();
                    j.Value.ToInstall = j.Value.Installed;
                }
            }
        }

        public static void LaunchGame()
        {
            Directory.SetCurrentDirectory(Shared.WorkingDir + "\\Game\\");
            System.Diagnostics.Process.Start(Shared.LauncherPath);
            Directory.SetCurrentDirectory(Shared.WorkingDir);
        }
    }
}
