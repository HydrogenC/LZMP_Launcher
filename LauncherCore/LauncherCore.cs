using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace LauncherCore
{

    public class MinecraftInstance
    {
        public static String WorkingPath = Directory.GetCurrentDirectory();
        public String GamePath;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gamePath">Relative path of the game (no backslash at the end)</param>
        /// <param name="launcherPath">Relative path of the launcher</param>
        public MinecraftInstance(String gamePath, String launcherPath)
        {
            GamePath = WorkingPath + "\\" + gamePath;
            LauncherPath = WorkingPath + "\\" + launcherPath;
        }

        public static Boolean operator ==(MinecraftInstance a, MinecraftInstance b)
        {
            return (a.GamePath == b.GamePath) && (a.LauncherPath == b.LauncherPath);
        }

        public static Boolean operator !=(MinecraftInstance a, MinecraftInstance b)
        {
            return (a.GamePath != b.GamePath) || (a.LauncherPath != b.LauncherPath);
        }

        public static String ResourcePath
        {
            get => WorkingPath + "\\Resources\\";
        }

        public String ModPath
        {
            get => GamePath + "\\mods\\";
        }

        public String ScriptPath
        {
            get => GamePath + "\\scripts\\";
        }

        public String LauncherPath;

        public override Boolean Equals(object obj)
        {
            return obj is MinecraftInstance instance && (GamePath == instance.GamePath) && (LauncherPath == instance.LauncherPath);
        }

        public override Int32 GetHashCode()
        {
            var hashCode = -1660369126;
            hashCode = hashCode * -1521134295 + EqualityComparer<String>.Default.GetHashCode(GamePath);
            hashCode = hashCode * -1521134295 + EqualityComparer<String>.Default.GetHashCode(LauncherPath);
            return hashCode;
        }
    }

    public struct SharedData
    {
        public static String Version;
        public static Dictionary<String, Mod> Mods = new Dictionary<String, Mod>();
        public static MinecraftInstance Client = new MinecraftInstance("Client\\.minecraft", "");
        public static MinecraftInstance Server = new MinecraftInstance("Server\\panel\\server", "");
        public static IWin32Window MainWindow;

        public static String SavePath
        {
            get => Client.GamePath + "\\saves\\";
        }

        public static String JMDataPath
        {
            get => Client.GamePath + "\\journeymap\\data\\sp\\";
        }
    }

    public struct SavesStatus
    {
        public static void Initialize()
        {
            status = "";
        }

        public static String status = "";
    }

    public struct ApplyProgress
    {
        public static void Initialize()
        {
            total = 0;
            current = 0;
        }

        public static Int32 total = 0, current = 0;
    }

    public class Core
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="check">If true, then check all mods. If false, cancel all mods. </param>
        public static void CheckAll(Boolean check = true)
        {
            foreach (var i in SharedData.Mods)
            {
                i.Value.ToInstall = check;

                foreach (var j in i.Value.Addons)
                {
                    j.Value.ToInstall = check;
                }
            }
        }

        public static void ApplyChanges(MinecraftInstance instance)
        {
            List<Mod> applyList = new List<Mod>();
            foreach (var i in SharedData.Mods)
            {
                if ((i.Value.ToInstall != i.Value.Installed[instance]) && i.Value.Available)
                {
                    applyList.Add(i.Value);
                }

                foreach (var j in i.Value.Addons)
                {
                    if ((j.Value.ToInstall != j.Value.Installed[instance]) && j.Value.Available)
                    {
                        applyList.Add(j.Value);
                    }
                }
            }

            if (applyList.Count == 0)
            {
                return;
            }

            ApplyProgress.total = applyList.Count;
            ApplyProgress.current = 0;

            foreach (var i in applyList)
            {

                if (i.Installed[instance])
                {
                    i.Uninstall(instance);
                }
                else
                {
                    i.Install(instance);
                }
                ApplyProgress.current += 1;
            }

            CheckInstallation();
            ApplyProgress.Initialize();
        }

        public static void CleanUp()
        {
            if (!Directory.Exists(MinecraftInstance.ResourcePath))
            {
                return;
            }

            String[] files = Directory.GetFiles(MinecraftInstance.ResourcePath);
            foreach (var i in files)
            {
                Boolean used = false;
                foreach (var j in SharedData.Mods)
                {
                    foreach (var k in j.Value.Addons)
                    {
                        if (k.Value.Files.Contains(Path.GetFileNameWithoutExtension(i)))
                        {
                            used = true;
                            break;
                        }
                    }
                    if (j.Value.Files.Contains(Path.GetFileNameWithoutExtension(i)))
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
                        MessageBox.Show(SharedData.MainWindow, "An error occured while cleaning! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            foreach (var i in SharedData.Mods)
            {
                i.Value.CheckInstalled(SharedData.Client);
                i.Value.CheckInstalled(SharedData.Server);

                foreach (var j in i.Value.Addons)
                {
                    j.Value.CheckInstalled(SharedData.Client);
                    j.Value.CheckInstalled(SharedData.Server);
                }
            }
        }

        public static void CheckToInstallState(MinecraftInstance instance)
        {
            foreach (var i in SharedData.Mods)
            {
                i.Value.ToInstall = i.Value.Installed[instance];

                foreach (var j in i.Value.Addons)
                {
                    j.Value.ToInstall = j.Value.Installed[instance];
                }
            }
        }

        public static void CheckAvailability()
        {
            foreach (var i in SharedData.Mods)
            {
                i.Value.CheckAvailability();
                foreach (var j in i.Value.Addons)
                {
                    j.Value.CheckAvailability();
                }
            }
        }

        public static void LaunchGame(MinecraftInstance instance)
        {
            Directory.SetCurrentDirectory(Path.GetDirectoryName(instance.LauncherPath));
            System.Diagnostics.Process.Start(instance.LauncherPath);
            Directory.SetCurrentDirectory(MinecraftInstance.WorkingPath);
        }
    }
}
