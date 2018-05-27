using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace WOT_Launcher
{
    public class GameType
    {
        private String typeName;
        private String baseDirectory;
        private ProcessStartInfo launcher = new ProcessStartInfo();

        public GameType(String name, String baseDir, String launcherDir)
        {
            typeName = name;
            baseDirectory = System.IO.Directory.GetCurrentDirectory() + "\\" + baseDir;
            launcher.FileName = System.IO.Directory.GetCurrentDirectory() + "\\" + launcherDir;
        }

        public String TypeName
        {
            get => typeName;
            set => typeName = value;
        }

        public String ModDirectory
        {
            get => baseDirectory + "mods\\";
        }

        public ProcessStartInfo Launcher
        {
            get => launcher;
            set => launcher = value;
        }

        public String BaseDirectory
        {
            get => baseDirectory;
        }

        public static GameType Client
        {
            get => new GameType("Client", "Client\\.minecraft\\", "MCCL.exe");
        }

        public static GameType Server
        {
            get => new GameType("Server", "Server\\", "Server\\MCserver_5.4.exe");
        }
    }
}
