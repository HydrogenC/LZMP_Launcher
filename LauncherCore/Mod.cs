using System;
using System.Collections.Generic;
using System.IO;

namespace LauncherCore
{
    public class Mod
    {
        private List<string> files = new List<string>();
        private Dictionary<string, Mod> addons = new Dictionary<string, Mod>();
        private bool installState = false;
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

        public void CheckInstalled()
        {
            Installed = true;

            foreach (var i in Files)
            {
                Installed = File.Exists(SharedData.ModPath + i + ".jar");
                if (!Installed)
                {
                    break;
                }
            }
        }

        public bool CheckAvailability()
        {
            Available = true;

            foreach (var i in files)
            {
                Available = File.Exists(SharedData.ResourcePath + i + ".jar");
                if (!Available)
                {
                    break;
                }
            }
            return Available;
        }

        public void Install()
        {
            foreach (var i in files)
            {
                try
                {
                    File.Copy(SharedData.ResourcePath + i + ".jar", SharedData.ModPath + i + ".jar");
                }
                catch (Exception e)
                {
                    SharedData.DisplayMessage("An error occured while installing: " + Name + "\nDetails: \n" + e.ToString(), "Error", MessageType.Error);
                    return;
                }
            }
        }

        public void Uninstall()
        {
            foreach (var i in files)
            {
                try
                {
                    File.Delete(SharedData.ModPath + i + ".jar");
                }
                catch (Exception e)
                {
                    SharedData.DisplayMessage("An error occured while uninstalling: " + Name + "\nDetails: \n" + e.ToString(), "Error", MessageType.Error);
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

        public ref bool Installed
        {
            get => ref installState;
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
