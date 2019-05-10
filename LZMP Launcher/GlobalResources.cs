using System;
using System.Collections.Generic;

using Direct = System.IO.Directory;

namespace LZMP_Launcher
{
    public struct Shared
    {
        public static readonly String workingDir = Direct.GetCurrentDirectory();
        public static readonly String resourceDir = workingDir + "\\Resources\\";
        public static readonly String clientModDir = workingDir + "\\Client\\.minecraft\\mods\\";
        public static readonly String serverModDir = workingDir + "\\Server\\panel\\server\\mods\\";
        public static String clientLauncher, serverLauncher, version;
        public static Dictionary<String, Mod> mods = new Dictionary<String, Mod>();
    }

    public enum ModCategory
    {
        Addon,
        Technology,
        Warfare,
        Enhancement
    }

    class Cleaner
    {
        private static String GetFileName(String fullPath)
        {
            return fullPath.Substring(fullPath.LastIndexOf('\\') + 1).Replace(".jar", "");
        }

        public static void CleanUp()
        {
            String[] files = Direct.GetFiles(Shared.resourceDir);
            foreach (var i in files)
            {
                Boolean used = false;
                foreach (var j in Shared.mods)
                {
                    if (j.Value.Files.Contains(GetFileName(i)))
                    {
                        used = true;
                        break;
                    }
                }

                if (!used)
                {
                    try
                    {
                        System.IO.File.Delete(i);
                    }
                    catch (Exception)
                    {
                        System.Windows.Forms.MessageBox.Show("An error occured while cleaning! ", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
