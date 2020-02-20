using LauncherCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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
    public class MainTreeItem : TreeViewItem, INotifyPropertyChanged
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

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
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
                bool? intend = null;
                switch (value)
                {
                    case CheckBoxState.Checked:
                        intend = true;
                        break;
                    case CheckBoxState.NotChecked:
                        intend = false;
                        break;
                    case CheckBoxState.HalfChecked:
                        intend = null;
                        break;
                }

                if (intend == itemChecked)
                {
                    return;
                }

                itemChecked = intend;
                UpdateParent();
                AfterCheck();
                OnPropertyChanged("itemChecked");
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

        public uint NumberOfChildrenChecked
        {
            get;
            set;
        } = 0u;

        public bool? ItemChecked
        {
            get => itemChecked;
            set
            {
                if (itemChecked == value)
                {
                    return;
                }

                itemChecked = value;
                UpdateParent();
                AfterCheck();
                OnPropertyChanged("itemChecked");
            }
        }

        public bool? ItemCheckedNoCheck
        {
            get => itemChecked;
            set
            {
                if (itemChecked == value)
                {
                    return;
                }

                itemChecked = value;
                UpdateParent();
                OnPropertyChanged("itemChecked");
            }
        }

        private void CheckAllChildren(bool flag)
        {
            foreach (var i in Children)
            {
                i.ItemCheckedNoCheck = flag;
            }
        }

        private void UpdateParent()
        {
            if (Parent != null && Parent.IsCategory)
            {
                if (ItemCheckedNoCheck.Value)
                {
                    Parent.NumberOfChildrenChecked += 1u;
                }
                else
                {
                    Parent.NumberOfChildrenChecked -= 1u;
                }
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
            else
            {
                if (Checked == CheckBoxState.NotChecked)
                {
                    CheckAllChildren(false);
                }
            }

            if (Parent.IsCategory)
            {
                if (Parent.NumberOfChildrenChecked == Parent.Children.Count)
                {
                    Parent.ItemCheckedNoCheck = true;
                    return;
                }
                if (Parent.NumberOfChildrenChecked == 0u)
                {
                    Parent.ItemCheckedNoCheck = false;
                    return;
                }
                Parent.ItemCheckedNoCheck = null;
            }
            else
            {
                if (Checked == CheckBoxState.Checked)
                {
                    Parent.ItemChecked = true;
                }
            }
        }
    }
}
