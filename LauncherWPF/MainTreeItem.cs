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
                if (Parent != null)
                {
                    UpdateNumberOfChildrenChecked(ItemCheckedRaw.Value);
                }
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

        private int NumberOfChildrenChecked = 0;
        public void UpdateNumberOfChildrenChecked(bool isChecked)
        {
            NumberOfChildrenChecked += isChecked ? 1 : -1;

            if (Parent != null)
            {
                Parent.UpdateNumberOfChildrenChecked(isChecked);
            }
        }

        public int TotalNumberOfChildren
        {
            get
            {
                int count = Children.Count;
                foreach (var i in Children)
                {
                    count += i.TotalNumberOfChildren;
                }

                return count;
            }
        }

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
                if (Parent != null)
                {
                    UpdateNumberOfChildrenChecked(ItemCheckedRaw.Value);
                }
                AfterCheck();
                OnPropertyChanged("itemChecked");
            }
        }

        public bool? ItemCheckedRaw
        {
            get => itemChecked;
            set
            {
                if (itemChecked == value)
                {
                    return;
                }

                itemChecked = value;
                if (Parent != null)
                {
                    UpdateNumberOfChildrenChecked(ItemCheckedRaw.Value);
                }
                OnPropertyChanged("itemChecked");
            }
        }

        private void CheckAllChildren(bool flag)
        {
            foreach (var i in Children)
            {
                i.ItemCheckedRaw = flag;
                if (i.Children.Count != 0)
                {
                    i.CheckAllChildren(flag);
                }
            }
        }

        private void UpdateParent()
        {

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
                if (Parent.NumberOfChildrenChecked == Parent.TotalNumberOfChildren)
                {
                    Parent.ItemCheckedRaw = true;
                    return;
                }
                if (Parent.NumberOfChildrenChecked == 0u)
                {
                    Parent.ItemCheckedRaw = false;
                    return;
                }
                Parent.ItemCheckedRaw = null;
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
