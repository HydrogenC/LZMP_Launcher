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
        private Boolean installed, available;
        private ModCategory category;
        private Dictionary<String, Mod> addons = new Dictionary<String, Mod>();
        private String name;

        public Mod(String name, ModCategory category = ModCategory.Addon, List<String> files = null, Dictionary<String, Mod> addons = null)
        {
            this.name = name;
            this.category = category;

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
            installed = true;

            foreach (var i in Files)
            {
                installed = File.Exists(Shared.clientModDir + i + ".jar") && System.IO.File.Exists(Shared.serverModDir + i + ".jar");
                if (!installed)
                {
                    break;
                }
            }
        }

        public void CheckAvailability()
        {
            available = true;

            foreach (var i in files)
            {
                available = File.Exists(Shared.resourceDir + i + ".jar");
                if (!available)
                {
                    MessageBox.Show("File not found: \n" + name, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    File.Copy(Shared.resourceDir + i + ".jar", Shared.clientModDir + i + ".jar");
                    File.Copy(Shared.resourceDir + i + ".jar", Shared.serverModDir + i + ".jar");
                }
                catch (Exception)
                {
                    MessageBox.Show("An error occured while installing: \n"+name, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    File.Delete(Shared.clientModDir + i + ".jar");
                    File.Delete(Shared.serverModDir + i + ".jar");
                }
                catch (Exception)
                {
                    MessageBox.Show("An error occured while uninstalling: \n" + name, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        public void AddNode(TreeNodeCollection nodes)
        {
            node = new TreeNode(name);
            nodes.Add(node);
            foreach (var i in addons)
            {
                i.Value.AddNode(node.Nodes);
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
            get => name;
            set => name = value;
        }

        public Boolean Installed
        {
            get => installed;
        }

        public Boolean Available
        {
            get => available;
        }

        public ModCategory Category
        {
            get => category;
            set => category = value;
        }
    }
}
