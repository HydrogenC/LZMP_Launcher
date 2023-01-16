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
        private bool allChecked = false, processing = false;
        public Dictionary<string, MainTreeItem> itemDict = new Dictionary<string, MainTreeItem>();
        public Dictionary<string, MainTreeItem> categoryDict = new Dictionary<string, MainTreeItem>();
        private ListDisplay currentListDisplay;

        public static SolidColorBrush unselectedForeground = new SolidColorBrush(Colors.White);
        public static SolidColorBrush selectedForeground = new SolidColorBrush(Color.FromRgb(0xE3, 0x21, 0x21));

        enum ListDisplay
        {
            Maps,
            Modsets
        }

        string FormatUnknownException(Exception e)
        {
            return $"Unknown {e.GetType().Name} caught: \n{e.Message}\n{e.StackTrace}";
        }

        private void ToggleListDisplay(ListDisplay display)
        {
            currentListDisplay = display;
            switch (display)
            {
                case ListDisplay.Maps:
                    MapsButton.Foreground = selectedForeground;
                    ModsetsButton.Foreground = unselectedForeground;
                    ModsetsButton.IsEnabled = true;
                    MapsButton.IsEnabled = false;
                    break;
                case ListDisplay.Modsets:
                    ModsetsButton.Foreground = selectedForeground;
                    MapsButton.Foreground = unselectedForeground;
                    MapsButton.IsEnabled = true;
                    ModsetsButton.IsEnabled = false;
                    break;
            }
            RefreshButton_Click(null, null);
        }

        public ModernUI()
        {
            InitializeComponent();
            SharedData.LogMessage = (string content, string caption, MessageType type) =>
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
            Core.CheckToInstallState();
            ToggleListDisplay(ListDisplay.Maps);
        }

        private void LaunchButton_Click(object sender, RoutedEventArgs e)
        {
            Core.ApplyChanges();
            Core.LaunchGame();
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
            EditableObject[] ie = null;

            try
            {
                switch (currentListDisplay)
                {
                    case ListDisplay.Maps:
                        ie = Scanner.ScanForMaps();
                        break;
                    case ListDisplay.Modsets:
                        ie = Scanner.ScanForModsets();
                        break;
                }
            }
            catch (Exception ex)
            {
                SharedData.LogMessage("Refresh failed: \n" + ex.Message, "Error", MessageType.Error);
                return;
            }

            foreach (var i in ie)
            {
                MainListBox.Items.Add(i);
            }
        }

        private bool NullShield()
        {
            if (MainListBox.SelectedItem == null || (!(MainListBox.SelectedItem is EditableObject)))
            {
                SharedData.LogMessage("You should select an item in the list to operate. ", "Info", MessageType.Info);
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

            if (currentListDisplay == ListDisplay.Modsets && MainListBox.SelectedIndex == 0)
            {
                return;
            }

            if (SharedData.LogMessage("Are you sure to delete, you cannot revert this! ", "Warning", MessageType.YesNoQuestion) == MessageResult.Yes)
            {
                EditableObject editable = MainListBox.SelectedItem as EditableObject;
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

            EditableObject target = MainListBox.SelectedItem as EditableObject;
            if (currentListDisplay == ListDisplay.Modsets && MainListBox.SelectedIndex == 0)
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

            EditableObject editable = MainListBox.SelectedItem as EditableObject;
            OpenFileDialog openFile = new OpenFileDialog()
            {
                Filter = currentListDisplay == ListDisplay.Maps ? (new Save()).IOFilter : (new Modset()).IOFilter
            };

            if (openFile.ShowDialog().Value == true)
            {
                processing = true;
                Action action = currentListDisplay switch
                {
                    ListDisplay.Maps => () => Save.ImportFrom(openFile.FileName),
                    ListDisplay.Modsets => () => Modset.ImportFrom(openFile.FileName),
                    _ => null
                };

                try
                {
                    Task tsk = Task.Run(action);
                    CurrentProgress.status = SharedData.Title;
                    while (!tsk.IsCompleted)
                    {
                        if ((string)TitleLabel.Content != CurrentProgress.status)
                        {
                            TitleLabel.Content = CurrentProgress.status;
                        }
                        DispatcherHelper.DoEvents();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(FormatUnknownException(ex), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

            EditableObject editable = MainListBox.SelectedItem as EditableObject;
            if (currentListDisplay == ListDisplay.Modsets && MainListBox.SelectedIndex == 0)
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
                    Task tsk = Task.Run(() => editable.ExportTo(saveFile.FileName));
                    CurrentProgress.status = SharedData.Title;
                    while (!tsk.IsCompleted)
                    {
                        if ((string)TitleLabel.Content != CurrentProgress.status)
                        {
                            TitleLabel.Content = CurrentProgress.status;
                        }
                        DispatcherHelper.DoEvents();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(FormatUnknownException(ex), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                Task tsk = Task.Run(Core.ApplyAction);
                while (!tsk.IsCompleted)
                {
                    if ((string)TitleLabel.Content != CurrentProgress.status)
                    {
                        TitleLabel.Content = CurrentProgress.status;
                    }
                    DispatcherHelper.DoEvents();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(FormatUnknownException(ex), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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