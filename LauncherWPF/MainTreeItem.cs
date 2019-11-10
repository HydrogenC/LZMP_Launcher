using LauncherCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LauncherWPF
{
    public class MainTreeItem
    {
        public MainTreeItem(string text)
        {
            Text = text;
        }

        public string Text
        {
            get;
            set;
        } = string.Empty;

        public List<MainTreeItem> Children
        {
            get;
        } = new List<MainTreeItem>();

        public bool? Checked
        {
            get;
            set;
        }

        public bool IsCategory
        {
            get;
            set;
        } = false;
    }
}
