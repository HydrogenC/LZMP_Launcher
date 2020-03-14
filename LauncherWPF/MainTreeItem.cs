using System.Collections.Generic;
using System.ComponentModel;

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
                    Parent.UpdateNumberOfChildrenChecked(ItemCheckedRaw.Value);
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

#if DEBUG
            System.Windows.MessageBox.Show(NumberOfChildrenChecked + " of " + TotalNumberOfChildren);
#endif
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
                    Parent.UpdateNumberOfChildrenChecked(ItemCheckedRaw.Value);
                }
                AfterCheck();
                OnPropertyChanged("itemChecked");
            }
        }

        private bool? ItemCheckedRaw
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
                    Parent.UpdateNumberOfChildrenChecked(ItemCheckedRaw.Value);
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

        private MainTreeItem GetCategory()
        {
            return IsCategory ? this : Parent.GetCategory();
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

            if (!Parent.IsCategory)
            {
                if (Checked == CheckBoxState.Checked)
                {
                    Parent.ItemChecked = true;
                }
            }

            MainTreeItem parentItem = GetCategory();
            if (parentItem.NumberOfChildrenChecked == parentItem.TotalNumberOfChildren)
            {
                parentItem.ItemCheckedRaw = true;
                return;
            }
            if (parentItem.NumberOfChildrenChecked == 0u)
            {
                parentItem.ItemCheckedRaw = false;
                return;
            }
            parentItem.ItemCheckedRaw = null;
        }
    }
}
