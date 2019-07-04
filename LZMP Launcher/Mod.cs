using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace LZMP_Launcher
{
    public class Mod
    {
        private List<String> files = new List<String>();
        private TreeNode node;
        private Dictionary<String, Mod> addons = new Dictionary<String, Mod>();

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

        public void CheckInstalled()
        {
            Installed = true;

            foreach (var i in Files)
            {
                Installed = File.Exists(Shared.modDir + i + ".jar");
                if (!Installed)
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
                Available = File.Exists(Shared.resourceDir + i + ".jar");
                if (!Available)
                {
                    MessageBox.Show("File not found: \n" + Name, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                }
            }
        }

        public void Install()
        {
            foreach (var i in files)
            {
                try
                {
                    File.Copy(Shared.resourceDir + i + ".jar", Shared.modDir + i + ".jar");
                }
                catch (Exception)
                {
                    MessageBox.Show("An error occured while installing: \n" + Name, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    File.Delete(Shared.modDir + i + ".jar");
                }
                catch (Exception)
                {
                    MessageBox.Show("An error occured while uninstalling: \n" + Name, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        public void CreateNode()
        {
            node = new TreeNode(Name);
            foreach (var i in addons)
            {
                i.Value.CreateNode();
                node.Nodes.Add(i.Value.Node);
            }
        }

        public ref TreeNode Node
        {
            get => ref node;
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

        public Boolean Installed
        {
            get;
            private set;
        }

        public Boolean Available
        {
            get;
            private set;
        }
    }
}
