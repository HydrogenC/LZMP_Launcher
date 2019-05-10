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
        private Boolean installed, available, coreMod;
        private UInt16 category = 0;
        private Dictionary<String, Mod> addons = new Dictionary<String, Mod>();
        private String name;

        public Mod(String name, UInt16 category = 0, List<String> files = null, Dictionary<String, Mod> addons = null)
        {
            this.name = name;
            this.coreMod = false;
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

        public Mod(String name, Boolean coreMod, UInt16 category = 0, List<String> files = null, Dictionary<String, Mod> addons = null)
        {
            this.name = name;
            this.coreMod = coreMod;
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
                installed = System.IO.File.Exists(GlobalResources.clientModDir + i + ".jar") && System.IO.File.Exists(GlobalResources.serverModDir + i + ".jar");
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
                available = System.IO.File.Exists(GlobalResources.resourceDir + i + ".jar");
                if (!available)
                {
                    MessageBox.Show("File not found: \n" + i, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                }
            }
        }

        public void Install()
        {
            String client, server;

            if (coreMod)
            {
                client = GlobalResources.clientCoreModDir;
                server = GlobalResources.serverCoreModDir;
            }
            else
            {
                client = GlobalResources.clientModDir;
                server = GlobalResources.serverModDir;
            }

            foreach (var i in files)
            {
                try
                {
                    File.Copy(GlobalResources.resourceDir + i + ".jar", client + i + ".jar");
                    File.Copy(GlobalResources.resourceDir + i + ".jar", server + i + ".jar");
                }
                catch (Exception)
                {
                    MessageBox.Show("An internal error occured while copying files. ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        public void Uninstall()
        {
            String client, server;

            if (coreMod)
            {
                client = GlobalResources.clientCoreModDir;
                server = GlobalResources.serverCoreModDir;
            }
            else
            {
                client = GlobalResources.clientModDir;
                server = GlobalResources.serverModDir;
            }

            foreach (var i in files)
            {
                try
                {
                    File.Delete(client + i + ".jar");
                    File.Delete(server + i + ".jar");
                }
                catch (Exception)
                {
                    MessageBox.Show("An internal error occured while deleting files. ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        public UInt16 Category
        {
            get => category;
            set => category = value;
        }

        public Boolean CoreMod
        {
            get => coreMod;
            set => coreMod = value;
        }
    }
}
