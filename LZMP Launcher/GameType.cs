using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace LZMP_Launcher
{
    public class GameType
    {
        private String typeName;
        private String modDirectory;
        private ProcessStartInfo launcherDirectory = new ProcessStartInfo();

        public GameType(String name, String modDir, String launcherDir)
        {
            typeName = name;
            modDirectory = modDir;
            launcherDirectory.FileName = launcherDir;
        }

        public String TypeName
        {
            get => typeName;
            set => typeName = value;
        }

        public String ModDirectory
        {
            get => modDirectory;
        }

        public ProcessStartInfo LauncherDirectory
        {
            get => launcherDirectory;
            set => launcherDirectory = value;
        }

        public static GameType Client
        {
            get => new GameType("Client", MainForm.workingDir + "\\Client\\.minecraft\\mods\\", MainForm.workingDir + "\\NsisoLauncher.exe");
        }

        public static GameType Server
        {
            get => new GameType("Server", MainForm.workingDir + "\\Server\\panel\\server\\mods\\", MainForm.workingDir + "\\MCim.exe");
        }
    }
}
