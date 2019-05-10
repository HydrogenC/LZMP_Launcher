using System;

using Direct = System.IO.Directory;

namespace LZMP_Launcher
{
    public struct GlobalResources
    {
        public static readonly String workingDir = Direct.GetCurrentDirectory();
        public static readonly String resourceDir = workingDir + "\\Resources\\";
        public static readonly String clientModDir = workingDir + "\\Client\\.minecraft\\mods\\";
        public static readonly String serverModDir = workingDir + "\\Server\\panel\\server\\mods\\";
        public static readonly String clientCoreModDir = workingDir + "\\Client\\.minecraft\\mods\\1.12.2\\";
        public static readonly String serverCoreModDir = workingDir + "\\Server\\panel\\server\\mods\\1.12.2\\";
        public static String clientLauncher, serverLauncher, version;
    }

    public enum ModCategory
    {
        BuiltIn,
        Addon,
        Technology,
        Warfare,
        Enhancement
    }
}
