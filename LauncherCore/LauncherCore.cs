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

    public struct SharedData
    {
        public static string Version, Title, LauncherPath;
        public static Dictionary<string, Mod> Mods = new Dictionary<string, Mod>();
        public static Func<string, string, MessageType, MessageResult> DisplayMessage;

        public static string WorkingPath = Directory.GetCurrentDirectory();

        public static string GamePath
        {
            get => WorkingPath + "\\Game\\.minecraft";
        }

        public static string SavePath
        {
            get => GamePath + "\\saves\\";
        }

        public static string JMDataPath
        {
            get => GamePath + "\\journeymap\\data\\sp\\";
        }

        public static string ResourcePath
        {
            get => WorkingPath + "\\Resources\\";
        }

        public static string ModPath
        {
            get => GamePath + "\\mods\\";
        }

        public static string ScriptPath
        {
            get => GamePath + "\\scripts\\";
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

        public static void ApplyChanges()
        {
            List<Mod> applyList = new List<Mod>();
            foreach (var i in SharedData.Mods)
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

            uint index = 0;
            CurrentProgress.status = "Applying 0/" + applyList.Count;

            Parallel.ForEach(applyList, (Mod i) =>
            {
                try
                {
                    if (i.Installed)
                    {
                        i.Uninstall();
                    }
                    else
                    {
                        i.Install();
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
        public static readonly Action ApplyAction = ApplyChanges;

        public static void CleanUp()
        {
            if (!Directory.Exists(SharedData.ResourcePath))
            {
                return;
            }

            string msg = "";
            string[] files = Directory.GetFiles(SharedData.ResourcePath);
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
                        msg += "\n" + i;
                    }
                    catch (Exception)
                    {
                        SharedData.DisplayMessage("An error occured while cleaning! ", "Error", MessageType.Error);
                    }
                }
            }

            if (!string.IsNullOrEmpty(msg))
            {
                SharedData.DisplayMessage("Deleted unused files: " + msg, "Info", MessageType.Info);
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
                i.Value.CheckInstalled();

                foreach (var j in i.Value.Addons)
                {
                    j.Value.CheckInstalled();
                }
            }
        }

        public static void CheckToInstallState()
        {
            foreach (var i in SharedData.Mods)
            {
                i.Value.ToInstall = i.Value.Installed;

                foreach (var j in i.Value.Addons)
                {
                    j.Value.ToInstall = j.Value.Installed;
                }
            }
        }

        public static void CheckAvailability()
        {
            string msg = "";
            foreach (var i in SharedData.Mods)
            {
                if (!i.Value.CheckAvailability())
                {
                    msg += "\n" + i.Value.Name;
                }

                foreach (var j in i.Value.Addons)
                {
                    if (!j.Value.CheckAvailability())
                    {
                        msg += "\n" + j.Value.Name;
                    }
                }
            }
            if (!string.IsNullOrEmpty(msg))
            {
                SharedData.DisplayMessage("Source files of these mods cannot be found: " + msg, "Warning", MessageType.Warning);
            }
        }

        public static void LaunchGame()
        {
            try
            {
                Directory.SetCurrentDirectory(Path.GetDirectoryName(SharedData.LauncherPath));
                System.Diagnostics.Process.Start(SharedData.LauncherPath);
                Directory.SetCurrentDirectory(SharedData.WorkingPath);
            }
            catch (Exception)
            {
                SharedData.DisplayMessage("An error occurred while launching, the launcher could have been missing! ", "Error", MessageType.Error);
            }
        }
    }
}
