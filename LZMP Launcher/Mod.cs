using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace LZMP_Launcher
{
    public class Mod
    {
        private List<String> files = new List<String>();
        private TreeNode node;
        private Boolean installed = true;
        private Boolean available = true;
        private UInt16 category = 0;
        private Dictionary<String, Mod> addons = new Dictionary<String, Mod>();
        private String name;

        public Mod(String name, UInt16 category = 0, List<String> files = null, Dictionary<String, Mod> addons = null)
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
                available = System.IO.File.Exists(GlobalResources.resDir + i + ".jar");
                if (!available)
                {
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
                    File.Copy(GlobalResources.resDir + i + ".jar", GlobalResources.clientModDir + i + ".jar");
                    File.Copy(GlobalResources.resDir + i + ".jar", GlobalResources.serverModDir + i + ".jar");
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
            foreach (var i in files)
            {
                try
                {
                    File.Delete(GlobalResources.clientModDir + i + ".jar");
                    File.Delete(GlobalResources.serverModDir + i + ".jar");
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

        public TreeNode Node
        {
            get => node;
            set => node = value;
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
        public UInt16 Category {
            get => category;
            set => category = value;
        }
    }
}
