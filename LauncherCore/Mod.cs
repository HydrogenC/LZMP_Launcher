using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace LauncherCore
{
    public class Mod
    {
        private List<String> files = new List<String>();
        private Dictionary<String, Mod> addons = new Dictionary<String, Mod>();
        private Dictionary<MinecraftInstance, Boolean> installStates = new Dictionary<MinecraftInstance, Boolean>();
        public Mod(String name, List<String> files = null, Dictionary<String, Mod> addons = null)
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
                Installed[instance] = File.Exists(instance.ModDir + i + ".jar");
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
                Available = File.Exists(MinecraftInstance.ResourceDir + i + ".jar");
                if (!Available)
                {
                    MessageBox.Show("File not found: \n" + Name, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    File.Copy(MinecraftInstance.ResourceDir + i + ".jar", instance.ModDir + i + ".jar");
                }
                catch (Exception)
                {
                    MessageBox.Show("An error occured while installing: \n" + Name, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    File.Delete(instance.ModDir + i + ".jar");
                }
                catch (Exception)
                {
                    MessageBox.Show("An error occured while uninstalling: \n" + Name, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        public ref Dictionary<String, Mod> Addons
        {
            get => ref addons;
        }

        public ref List<String> Files
        {
            get => ref files;
        }

        public String Name
        {
            get;
            set;
        }

        public ref Dictionary<MinecraftInstance, Boolean> Installed
        {
            get => ref installStates;
        }

        public Boolean Available
        {
            get;
            private set;
        }

        public String Category
        {
            get;
            set;
        }

        public String Key
        {
            get;
            set;
        }

        public Boolean ToInstall
        {
            get => GetToInstallState(this);
            set => SetToInstallState(this, value);
        }

        public static Action<Mod, Boolean> SetToInstallState
        {
            private get;
            set;
        }

        public static Func<Mod, Boolean> GetToInstallState
        {
            private get;
            set;
        }

        public override String ToString()
        {
            return Name;
        }
    }
}
