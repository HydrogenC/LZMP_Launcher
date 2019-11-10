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
    public class MainTreeItem : INotifyPropertyChanged
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

        private bool? itemChecked;

        public bool? CheckedBinding
        {
            get => itemChecked;
            set => itemChecked = value;
        }

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

        public void CheckAllChildren(bool flag)
        {
            CheckBoxState state = CheckBoxState.NotChecked;
            if (flag)
            {
                state = CheckBoxState.Checked;
            }
            else
            {
                state = CheckBoxState.NotChecked;
            }

            foreach (var i in Children)
            {
                i.Checked = state;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = (object sender, PropertyChangedEventArgs args) =>
        {
            MainTreeItem item = (MainTreeItem)sender;

            if (item.IsCategory)
            {
                if (item.Checked == CheckBoxState.Checked)
                {
                    item.CheckAllChildren(true);
                    return;
                }

                if (item.Checked == CheckBoxState.NotChecked)
                {
                    item.CheckAllChildren(false);
                    return;
                }
            }

            if (item.Parent.IsCategory)
            {
                if (item.Parent.AllChildrenChecked)
                {
                    item.Parent.Checked = CheckBoxState.Checked;
                    return;
                }
                if (item.Parent.NoChildrenChecked)
                {
                    item.Parent.Checked = CheckBoxState.NotChecked;
                    return;
                }
                item.Parent.Checked = CheckBoxState.HalfChecked;
            }
            else
            {
                if (item.Checked == CheckBoxState.Checked)
                {
                    item.Parent.Checked = CheckBoxState.Checked;
                }
            }
        };

        private void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
}
