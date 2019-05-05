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
        private List<String> files;
        private TreeNode node;
        private Boolean installed = true;
        private Boolean available = true;
        private UInt16 category = 0;
        private Mod parentMod = null;
        private String name;

        public Mod(String name, List<String> files, Mod parentMod)
        {
            this.name = name;
            this.files = files;
            this.parentMod = parentMod;
        }

        public Mod(String name, List<String> files, UInt16 category)
        {
            this.name = name;
            this.files = files;
            this.category = category;
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

        public void AddNode(TreeView treeView)
        {
            TreeNode treeNode = new TreeNode(name);
            if (parentMod == null)
            {
                treeView.Nodes[category].Nodes.Add(treeNode);
            }
            else
            {
                parentMod.Node.Nodes.Add(treeNode);
            }
            node = treeNode;
        }

        public TreeNode Node
        {
            get => node;
            set => node = value;
        }

        public Mod ParentMod
        {
            get => parentMod;
            set => parentMod = value;
        }

        public String Name
        {
            get => name;
            set => name = value;
        }

        public List<String> Files
        {
            get => files;
            set => files = value;
        }

        public Boolean Installed
        {
            get => installed;
        }
        public bool Available
        {
            get => available;
        }
    }
}
