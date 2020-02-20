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

        public Mod() { }

        public Mod(string name, List<string> files = null, Dictionary<string, Mod> addons = null)
        {
            Name = name;

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

        public static bool FindMod(ref Dictionary<string, Mod> ie, string reqKey, out Mod mod)
        {
            foreach (var i in ie)
            {
                if (i.Key == reqKey)
                {
                    mod = i.Value;
                    return true;
                }
                else
                {
                    foreach (var j in i.Value.Addons)
                    {
                        if (j.Key == reqKey)
                        {
                            mod = j.Value;
                            return true;
                        }
                    }
                }
            }
            mod = new Mod();
            return false;
        }

        public override bool Equals(object obj)
        {
            return obj is Mod mod &&
                   EqualityComparer<Dictionary<string, Mod>>.Default.Equals(Addons, mod.Addons) &&
                   Name == mod.Name &&
                   Key == mod.Key;
        }

        public override int GetHashCode()
        {
            var hashCode = -2139369844;
            hashCode = hashCode * -1521134295 + EqualityComparer<Dictionary<string, Mod>>.Default.GetHashCode(Addons);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Key);
            return hashCode;
        }
    }
}
