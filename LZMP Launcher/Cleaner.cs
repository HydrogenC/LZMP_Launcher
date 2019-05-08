﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LZMP_Launcher
{
    class Cleaner
    {
        public static void CleanUp(ref Dictionary<String, Mod> dict)
        {
            String[] files = Directory.GetFiles(GlobalResources.resourceDir);
            foreach (var i in files)
            {
                Boolean used = false;
                foreach (var j in dict)
                {
                    if (j.Value.Files.Contains(i.Substring(i.LastIndexOf('\\') + 1).Replace(".jar", "")))
                    {
                        used = true;
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
                        System.Windows.Forms.MessageBox.Show("An error occured while cleaning! ", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error); ;
                    }
                }
            }
        }
    }
}
