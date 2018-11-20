﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WOT_Launcher
{
    public class Mod : Object
    {
        private List<String> files;
        private TreeNode node;
        private Boolean installed = true;
        private Boolean available = true;
        private Int16 category = 0;
        private Mod parentMod = null;
        private String name;

        public Mod(String name, List<String> files, Mod parentMod)
        {
            this.name = name;
            this.files = files;
            this.parentMod = parentMod;
        }

        public Mod(String name, List<String> files, Int16 category)
        {
            this.name = name;
            this.files = files;
            this.category = category;
        }

        public Boolean CheckInstalled(GameType gameType)
        {
            installed = true;
            available = true;
            foreach (var i in Files)
            {
                installed = System.IO.File.Exists(gameType.ModDirectory + i + ".jar");
                available = System.IO.File.Exists(System.IO.Directory.GetCurrentDirectory() + "\\Resources\\" + i + ".jar");
                if (!available)
                {
                    break;
                }
            }
            return installed;
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
