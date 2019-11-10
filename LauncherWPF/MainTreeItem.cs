using LauncherCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LauncherWPF
{
    public enum CheckBoxState
    {
        Checked,
        NotChecked,
        HalfChecked
    }

    /// <summary>
    /// MainTreeView 的交互逻辑
    /// </summary>
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

        public MainTreeItem Parent
        {
            get;
            set;
        } = null;

        public List<MainTreeItem> Children
        {
            get;
        } = new List<MainTreeItem>();

        private bool? itemChecked = false;

        public CheckBoxState Checked
        {
            get
            {
                if (itemChecked == null)
                {
                    return CheckBoxState.HalfChecked;
                }

                if (itemChecked.Value)
                {
                    return CheckBoxState.Checked;
                }
                else
                {
                    return CheckBoxState.NotChecked;
                }
            }

            set
            {
                switch (value)
                {
                    case CheckBoxState.Checked:
                        itemChecked = true;
                        break;
                    case CheckBoxState.NotChecked:
                        itemChecked = false;
                        break;
                    case CheckBoxState.HalfChecked:
                        itemChecked = null;
                        break;
                }
                AfterCheck();
            }
        }

        public bool IsCategory
        {
            get;
            set;
        } = false;

        public bool HasParent
        {
            get => Parent != null;
        }

        public bool NoChildrenChecked
        {
            get
            {
                bool noChecked = true;
                foreach (var i in Children)
                {
                    if (i.Checked == CheckBoxState.Checked)
                    {
                        noChecked = false;
                        break;
                    }
                }
                return noChecked;
            }
        }

        public bool AllChildrenChecked
        {
            get
            {
                bool allChecked = true;
                foreach (var i in Children)
                {
                    if (i.Checked != CheckBoxState.Checked)
                    {
                        allChecked = false;
                        break;
                    }
                }
                return allChecked;
            }
        }

        public bool? ItemChecked
        {
            get => itemChecked;
            set
            {
                itemChecked = value;
                AfterCheck();
            }
        }

        public void CheckAllChildren(bool flag)
        {
            foreach (var i in Children)
            {
                i.itemChecked = flag;
            }
        }

        private void AfterCheck()
        {
            if (IsCategory)
            {
                if (Checked == CheckBoxState.Checked)
                {
                    CheckAllChildren(true);
                    return;
                }

                if (Checked == CheckBoxState.NotChecked)
                {
                    CheckAllChildren(false);
                    return;
                }
            }

            if ((Parent != null) && Parent.IsCategory)
            {
                if (Parent.AllChildrenChecked)
                {
                    Parent.Checked = CheckBoxState.Checked;
                    return;
                }
                if (Parent.NoChildrenChecked)
                {
                    Parent.Checked = CheckBoxState.NotChecked;
                    return;
                }
                Parent.Checked = CheckBoxState.HalfChecked;
            }
            else
            {
                if (Checked == CheckBoxState.Checked)
                {
                    Parent.Checked = CheckBoxState.Checked;
                }
            }
        }
    }
}
