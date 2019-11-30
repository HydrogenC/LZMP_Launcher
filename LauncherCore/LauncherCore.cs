using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LauncherCore
{
    public enum MessageType
    {
        Error,
        Info,
        Warning,
        YesNoQuestion,
        YesNoCancelQuestion,
        OKCancelQuestion
    }

    public enum MessageResult
    {
        OK,
        Cancel,
        Yes,
        No
    }

    public class MinecraftInstance
    {
        public static string WorkingPath = Directory.GetCurrentDirectory();
        public string GamePath;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gamePath">Relative path of the game (no backslash at the end)</param>
        /// <param name="launcherPath">Relative path of the launcher</param>
        public MinecraftInstance(string gamePath, string launcherPath)
        {
            GamePath = WorkingPath + "\\" + gamePath;
            LauncherPath = WorkingPath + "\\" + launcherPath;
        }

        public static bool operator ==(MinecraftInstance a, MinecraftInstance b)
        {
            return (a.GamePath == b.GamePath) && (a.LauncherPath == b.LauncherPath);
        }

        public static bool operator !=(MinecraftInstance a, MinecraftInstance b)
        {
            return (a.GamePath != b.GamePath) || (a.LauncherPath != b.LauncherPath);
        }

        public static string ResourcePath
        {
            get => WorkingPath + "\\Resources\\";
        }

        public string ModPath
        {
            get => GamePath + "\\mods\\";
        }

        public string ScriptPath
        {
            get => GamePath + "\\scripts\\";
        }

        public string LauncherPath;

        public override bool Equals(object obj)
        {
            return obj is MinecraftInstance instance && (GamePath == instance.GamePath) && (LauncherPath == instance.LauncherPath);
        }

        public override Int32 GetHashCode()
        {
            var hashCode = -1660369126;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(GamePath);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(LauncherPath);
            return hashCode;
        }
    }

    public struct SharedData
    {
        public static string Version;
        public static Dictionary<string, Mod> Mods = new Dictionary<string, Mod>();
        public static MinecraftInstance Client = new MinecraftInstance("Client\\.minecraft", string.Empty);
        public static MinecraftInstance Server = new MinecraftInstance("Server\\panel\\server", string.Empty);
        public static Func<string, string, MessageType, MessageResult> DisplayMessage;

        public static string SavePath
        {
            get => Client.GamePath + "\\saves\\";
        }

        public static string JMDataPath
        {
            get => Client.GamePath + "\\journeymap\\data\\sp\\";
        }
    }

    public struct CurrentProgress
    {
        public static void Initialize()
        {
            status = string.Empty;
        }

        public static string status = string.Empty;
        public static Mutex mutex = new Mutex();
    }

    public class Core
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="check">If true, then check all mods. If false, cancel all mods. </param>
        public static void CheckAll(bool check = true)
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

            uint index = 0;
            CurrentProgress.status = "Applying 0/" + applyList.Count;

            Parallel.ForEach(applyList, (Mod i) =>
            {
                try
                {
                    if (i.Installed[instance])
                    {
                        i.Uninstall(instance);
                    }
                    else
                    {
                        i.Install(instance);
                    }
                }
                catch (Exception) { }

                CurrentProgress.mutex.WaitOne();
                index += 1;
                CurrentProgress.status = "Applying " + index + "/" + applyList.Count;
                CurrentProgress.mutex.ReleaseMutex();
            });

            CheckInstallation();
            CurrentProgress.Initialize();
        }
        public static readonly Action<MinecraftInstance> ApplyAction = ApplyChanges;

        public static void CleanUp()
        {
            if (!Directory.Exists(MinecraftInstance.ResourcePath))
            {
                return;
            }

            string[] files = Directory.GetFiles(MinecraftInstance.ResourcePath);
            foreach (var i in files)
            {
                bool used = false;
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
                        SharedData.DisplayMessage("An error occured while cleaning! ", "Error", MessageType.Error);
                    }
                }
            }
        }

        public static void CopyDirectory(string srcPath, string aimPath)
        {
            if (aimPath[aimPath.Length - 1] != '\\')
            {
                aimPath += '\\';
            }

            if (!Directory.Exists(aimPath))
            {
                Directory.CreateDirectory(aimPath);
            }

            foreach (string file in Directory.GetFileSystemEntries(srcPath))
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
