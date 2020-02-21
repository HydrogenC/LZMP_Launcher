using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using LauncherCore;
using Microsoft.Win32;

namespace LauncherWPF
{
    /// <summary>
    /// ModernUI.xaml 的交互逻辑
    /// </summary>
    public partial class ModernUI : Window
    {
        private bool allChecked = false, processing = false, setLoaded = false;
        public Dictionary<string, MainTreeItem> itemDict = new Dictionary<string, MainTreeItem>();
        public Dictionary<string, MainTreeItem> categoryDict = new Dictionary<string, MainTreeItem>();
        private ListDisplay currentLD;

        enum ListDisplay
        {
            Maps,
            Modsets
        }

        private void ToggleListDisplay(ListDisplay display)
        {
            currentLD = display;
            switch (display)
            {
                case ListDisplay.Maps:
                    MapsButton.Foreground = new SolidColorBrush(Colors.Black);
                    ModsetsButton.Foreground = new SolidColorBrush(Colors.White);
                    ModsetsButton.IsEnabled = true;
                    MapsButton.IsEnabled = false;
                    break;
                case ListDisplay.Modsets:
                    ModsetsButton.Foreground = new SolidColorBrush(Colors.Black);
                    MapsButton.Foreground = new SolidColorBrush(Colors.White);
                    MapsButton.IsEnabled = true;
                    ModsetsButton.IsEnabled = false;
                    break;
            }
            RefreshButton_Click(null, null);
        }

        public ModernUI()
        {
            InitializeComponent();
            SharedData.DisplayMessage = (string content, string caption, MessageType type) =>
            {
                MessageBoxButton btn = MessageBoxButton.OK;
                MessageBoxImage icn = MessageBoxImage.None;
                switch (type)
                {
                    case MessageType.Error:
                        btn = MessageBoxButton.OK;
                        icn = MessageBoxImage.Error;
                        break;
                    case MessageType.Info:
                        btn = MessageBoxButton.OK;
                        icn = MessageBoxImage.Information;
                        break;
                    case MessageType.Warning:
                        btn = MessageBoxButton.OK;
                        icn = MessageBoxImage.Warning;
                        break;
                    case MessageType.OKCancelQuestion:
                        btn = MessageBoxButton.OKCancel;
                        icn = MessageBoxImage.Question;
                        break;
                    case MessageType.YesNoQuestion:
                        btn = MessageBoxButton.YesNo;
                        icn = MessageBoxImage.Question;
                        break;
                    case MessageType.YesNoCancelQuestion:
                        btn = MessageBoxButton.YesNoCancel;
                        icn = MessageBoxImage.Question;
                        break;
                }

                MessageBoxResult rst = MessageBoxResult.OK;
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => rst = MessageBox.Show(this, content, caption, btn, icn)));

                switch (rst)
                {
                    case MessageBoxResult.OK:
                        return MessageResult.OK;
                    case MessageBoxResult.Cancel:
                        return MessageResult.Cancel;
                    case MessageBoxResult.Yes:
                        return MessageResult.Yes;
                    case MessageBoxResult.No:
                        return MessageResult.No;
                    default:
                        return MessageResult.OK;
                }
            };
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            Mod.GetToInstallState = (Mod mod) => itemDict[mod.Key].Checked == CheckBoxState.Checked;
            Mod.SetToInstallState = (Mod mod, bool flag) =>
            {
                if (flag)
                {
                    itemDict[mod.Key].Checked = CheckBoxState.Checked;
                }
                else
                {
                    itemDict[mod.Key].Checked = CheckBoxState.NotChecked;
                }
            };

            Scanner.ScanConfig(SharedData.WorkingPath + "\\BasicSettings.xml");
            TitleLabel.Content = SharedData.Title;
            Core.CheckInstallation();
            Core.CheckAvailability();

            WriteNodes();
            ToggleListDisplay(ListDisplay.Maps);
        }

        private void LaunchButton_Click(object sender, RoutedEventArgs e)
        {
            Core.ApplyChanges();
            Core.LaunchGame();
        }

        private void ProcessEndCallback(IAsyncResult ar)
        {
            processing = false;
        }

        private void CheckIfAllChecked()
        {
            allChecked = true;
            foreach (var i in SharedData.Mods)
            {
                if (!i.Value.ToInstall)
                {
                    allChecked = false;
                }

                foreach (var j in i.Value.Addons)
                {
                    if (!j.Value.ToInstall)
                    {
                        allChecked = false;
                        break;
                    }
                }

                if (!allChecked)
                {
                    break;
                }
            }

            if (allChecked)
            {
                CheckButton.Content = "Cancel All";
            }
            else
            {
                CheckButton.Content = "Check All";
            }
        }

        private void WriteNodes()
        {
            foreach (var i in SharedData.Mods)
            {
                if (!categoryDict.ContainsKey(i.Value.Category))
                {
                    categoryDict[i.Value.Category] = new MainTreeItem(i.Value.Category + " Mods")
                    {
                        IsCategory = true
                    };
                }

                itemDict[i.Key] = new MainTreeItem(i.Value.Name)
                {
                    IsCategory = false,
                    Parent = categoryDict[i.Value.Category]
                };
                categoryDict[i.Value.Category].Children.Add(itemDict[i.Key]);

                foreach (var j in i.Value.Addons)
                {
                    itemDict[j.Key] = new MainTreeItem(j.Value.Name)
                    {
                        IsCategory = false,
                        Parent = itemDict[i.Key]
                    };
                    itemDict[i.Key].Children.Add(itemDict[j.Key]);
                }
            }

            foreach (var i in categoryDict)
            {
                MainTreeView.Items.Add(i.Value);
            }
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            if (processing)
            {
                return;
            }

            Core.CheckAll(!allChecked);
            CheckIfAllChecked();
        }

        private void MapsButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleListDisplay(ListDisplay.Maps);
        }

        private void ModsetsButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleListDisplay(ListDisplay.Modsets);
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            MainListBox.Items.Clear();
            IEditable[] ie = null;

            try
            {
                switch (currentLD)
                {
                    case ListDisplay.Maps:
                        ie = Scanner.ScanForMaps();
                        break;
                    case ListDisplay.Modsets:
                        ie = Scanner.ScanForModsets();
                        break;
                }
            }
            catch (Exception)
            {
                SharedData.DisplayMessage("Refresh failed", "Error", MessageType.Error);
                return;
            }

            foreach (var i in ie)
            {
                MainListBox.Items.Add(i);
            }
        }

        private bool NullShield()
        {
            if (MainListBox.SelectedItem == null || (!(MainListBox.SelectedItem is IEditable)))
            {
                SharedData.DisplayMessage("You should select an item in the list to operate. ", "Info", MessageType.Info);
                return false;
            }
            return true;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if ((!NullShield()) || processing)
            {
                return;
            }

            if (currentLD == ListDisplay.Modsets && MainListBox.SelectedIndex == 0)
            {
                return;
            }

            if (SharedData.DisplayMessage("Are you sure to delete, you cannot revert this! ", "Warning", MessageType.YesNoQuestion) == MessageResult.Yes)
            {
                IEditable editable = MainListBox.SelectedItem as IEditable;
                editable.Delete();
                RefreshButton_Click(null, null);
            }
        }

        private void RenameButton_Click(object sender, RoutedEventArgs e)
        {
            if ((!NullShield()) || processing)
            {
                return;
            }

            IEditable target = MainListBox.SelectedItem as IEditable;
            if (currentLD == ListDisplay.Modsets && MainListBox.SelectedIndex == 0)
            {
                target = new Modset(ref SharedData.Mods, "New Modset");
            }

            RenameWindow rename = new RenameWindow(target);
            rename.ShowDialog();
            RefreshButton_Click(null, null);
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            if (processing)
            {
                return;
            }

            IEditable editable = MainListBox.SelectedItem as IEditable;
            OpenFileDialog openFile = new OpenFileDialog()
            {
                Filter = currentLD == ListDisplay.Maps ? (new Save()).IOFilter : (new Modset()).IOFilter
            };

            if (openFile.ShowDialog().Value == true)
            {
                processing = true;
                Action<string> action = null;
                switch (currentLD)
                {
                    case ListDisplay.Maps:
                        action = new Action<string>(Save.ImportFrom);
                        break;
                    case ListDisplay.Modsets:
                        action = new Action<string>(Modset.ImportFrom);
                        break;
                }

                try
                {
                    action.BeginInvoke(openFile.FileName, ProcessEndCallback, null);
                    CurrentProgress.status = SharedData.Title;
                    while (processing)
                    {
                        if ((string)TitleLabel.Content != CurrentProgress.status)
                        {
                            TitleLabel.Content = CurrentProgress.status;
                        }
                        DispatcherHelper.DoEvents();
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Unknown exception caught! ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    RefreshButton_Click(null, null);
                    processing = false;
                    TitleLabel.Content = SharedData.Title;
                }
            }
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            if ((!NullShield()) || processing)
            {
                return;
            }

            IEditable editable = MainListBox.SelectedItem as IEditable;
            if (currentLD == ListDisplay.Modsets && MainListBox.SelectedIndex == 0)
            {
                editable = new Modset(ref SharedData.Mods, "New Modset");
            }

            SaveFileDialog saveFile = new SaveFileDialog()
            {
                Filter = editable.IOFilter
            };

            if (saveFile.ShowDialog().Value == true)
            {
                processing = true;
                Action<string> action = new Action<string>(editable.ExportTo);

                try
                {
                    action.BeginInvoke(saveFile.FileName, ProcessEndCallback, null);
                    CurrentProgress.status = SharedData.Title;
                    while (processing)
                    {
                        if ((string)TitleLabel.Content != CurrentProgress.status)
                        {
                            TitleLabel.Content = CurrentProgress.status;
                        }
                        DispatcherHelper.DoEvents();
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Unknown exception caught! ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    RefreshButton_Click(null, null);
                    processing = false;
                    TitleLabel.Content = SharedData.Title;
                }
            }
        }

        private void MainListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((!NullShield()) || (!(MainListBox.SelectedItem is Modset)))
            {
                return;
            }

            if (MainListBox.SelectedIndex != 0)
            {
                (MainListBox.SelectedItem as Modset).Apply();
            }
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            if (processing)
            {
                return;
            }

            processing = true;

            try
            {
                processing = true;
                Core.ApplyAction.BeginInvoke(ProcessEndCallback, null);

                while (processing)
                {
                    if ((string)TitleLabel.Content != CurrentProgress.status)
                    {
                        TitleLabel.Content = CurrentProgress.status;
                    }
                    DispatcherHelper.DoEvents();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Unknown exception caught! ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Core.CheckToInstallState();
                processing = false;
                TitleLabel.Content = SharedData.Title;
            }
        }
    }
}
