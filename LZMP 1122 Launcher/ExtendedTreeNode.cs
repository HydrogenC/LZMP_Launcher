using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LZMP_Launcher
{
    class ExtendedTreeNode : System.Windows.Forms.TreeNode
    {
        private Mod relMod;

        public ExtendedTreeNode() : base() { }

        public ExtendedTreeNode(Mod relatedMod) : base()
        {
            relMod = relatedMod;
        }

        public ExtendedTreeNode(String txt) : base(txt) { }

        public ExtendedTreeNode(String txt, Mod relatedMod) : base(txt)
        {
            relMod = relatedMod;
        }

        public Mod RelMod
        {
            get => relMod;
            set => relMod = value;
        }
    }
}
