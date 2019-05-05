using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Direct = System.IO.Directory;

namespace LZMP_Launcher
{
    public struct GlobalResources
    {
        public static readonly String workingDir = Direct.GetCurrentDirectory();
        public static readonly String resDir = workingDir + "\\Resources\\";
        public static readonly String clientModDir = workingDir + "\\Client\\.minecraft\\mods\\";
        public static readonly String serverModDir = workingDir + "\\Server\\panel\\server\\mods\\";
        public static String clientLauncher, serverLauncher;
    }
}
