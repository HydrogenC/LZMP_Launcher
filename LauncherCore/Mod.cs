using System;
using System.Collections.Generic;
using System.IO;

namespace LauncherCore
{
    public class Mod
    {
        private List<string> files = new List<string>();
        private Dictionary<string, Mod> addons = new Dictionary<string, Mod>();
        private Dictionary<MinecraftInstance, bool> installStates = new Dictionary<MinecraftInstance, bool>();
        public Mod(string name, List<string> files = null, Dictionary<string, Mod> addons = null)
        {
            this.Name = name;

            if (files != null)
            {
                this.files = files;
            }

            if (addons != null)
            {
                this.addons = addons;
            }
        }

        public void CheckInstalled(MinecraftInstance instance)
        {
            Installed[instance] = true;

            foreach (var i in Files)
            {
                Installed[instance] = File.Exists(instance.ModPath + i + ".jar");
                if (!Installed[instance])
                {
                    break;
                }
            }
        }

        public void CheckAvailability()
        {
            Available = true;

            foreach (var i in files)
            {
                Available = File.Exists(MinecraftInstance.ResourcePath + i + ".jar");
                if (!Available)
                {
                    SharedData.DisplayMessage("File not found: \n" + Name, "Warning", MessageType.Warning);
                    break;
                }
            }
        }

        public void Install(MinecraftInstance instance)
        {
            foreach (var i in files)
            {
                try
                {
                    File.Copy(MinecraftInstance.ResourcePath + i + ".jar", instance.ModPath + i + ".jar");
                }
                catch (Exception)
                {
                    SharedData.DisplayMessage("An error occured while installing: \n" + Name, "Error", MessageType.Error);
                    return;
                }
            }
        }

        public void Uninstall(MinecraftInstance instance)
        {
            foreach (var i in files)
            {
                try
                {
                    File.Delete(instance.ModPath + i + ".jar");
                }
                catch (Exception)
                {
                    SharedData.DisplayMessage("An error occured while uninstalling: \n" + Name, "Error", MessageType.Error);
                    return;
                }
            }
        }

        public ref Dictionary<string, Mod> Addons
        {
            get => ref addons;
        }

        public ref List<string> Files
        {
            get => ref files;
        }

        public string Name
        {
            get;
            set;
        }

        public ref Dictionary<MinecraftInstance, bool> Installed
        {
            get => ref installStates;
        }

        public bool Available
        {
            get;
            private set;
        }

        public string Category
        {
            get;
            set;
        }

        public string Key
        {
            get;
            set;
        }

        public bool ToInstall
        {
            get => GetToInstallState(this);
            set => SetToInstallState(this, value);
        }

        public static Action<Mod, bool> SetToInstallState
        {
            private get;
            set;
        }

        public static Func<Mod, bool> GetToInstallState
        {
            private get;
            set;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
