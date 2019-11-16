using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using LauncherCore;
using Microsoft.Win32;

namespace LauncherWPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
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
            SharedData.SaveFile = (string initial, string filter, string caption) =>
            {
                SaveFileDialog dialog = new SaveFileDialog();
                if (!string.IsNullOrEmpty(initial))
                {
                    dialog.FileName = initial;
                }
                if (!string.IsNullOrEmpty(filter))
                {
                    dialog.Filter = filter;
                }
                if (!string.IsNullOrEmpty(caption))
                {
                    dialog.Title = caption;
                }

                bool? rst = null;
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => rst = dialog.ShowDialog(this)));

                if (rst.HasValue ? rst.Value : false)
                {
                    return dialog.FileName;
                }
                else
                {
                    return null;
                }
            };
            App.SwitchPage = (dynamic page) =>
            {
                App.CurrentPage = page;
                PageFrame.Content = App.CurrentPage;
            };
            App.BusyAction = (bool isBusy) =>
            {
                if (isBusy)
                {
                    LauncherTitleLabel.Foreground = Brushes.LightYellow;
                }
                else
                {
                    LauncherTitleLabel.Foreground = Brushes.White;
                }
            };
            App.GetTitleText = () => (string)LauncherTitleLabel.Content;
            App.SetTitleText = (string a) => LauncherTitleLabel.Content = a;
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            Mod.GetToInstallState = (Mod mod) => App.MainModPage.itemDict[mod.Key].Checked == CheckBoxState.Checked;
            Mod.SetToInstallState = (Mod mod, bool flag) =>
            {
                if (flag)
                {
                    App.MainModPage.itemDict[mod.Key].Checked = CheckBoxState.Checked;
                }
                else
                {
                    App.MainModPage.itemDict[mod.Key].Checked = CheckBoxState.NotChecked;
                }
            };

            XmlHelper.ReadDefinitions(MinecraftInstance.WorkingPath + "\\BasicSettings.xml");
            App.DefaultTitle = string.Format(App.DefaultTitle, SharedData.Version);
            LauncherTitleLabel.Content = App.DefaultTitle;
            Core.CheckInstallation();

            if (Directory.Exists(MinecraftInstance.WorkingPath + "\\Mods"))
            {
                Core.CopyDirectory(MinecraftInstance.WorkingPath + "\\Mods", SharedData.Client.ModPath);
                Core.CopyDirectory(MinecraftInstance.WorkingPath + "\\Mods", SharedData.Server.ModPath);
                Directory.Delete(MinecraftInstance.WorkingPath + "\\Mods", true);
            }
            Core.CheckAvailability();
            ClientRadio.IsChecked = true;
            ClientCheck.IsChecked = true;
            App.ApplyForClient = true;
            ServerCheck.IsChecked = true;
            App.ApplyForServer = true;

            App.SwitchPage(new MenuPage());
            App.GeneratePages();
        }

        private void CloseForm_Click(object sender, RoutedEventArgs e)
        {
            if (!App.Busy)
            {
                Close();
            }
        }

        private void LaunchServerButton_Click(object sender, RoutedEventArgs e)
        {
            Core.ApplyChanges(SharedData.Server);
            Core.LaunchGame(SharedData.Server);
        }

        private void LaunchClientButton_Click(object sender, RoutedEventArgs e)
        {
            Core.ApplyChanges(SharedData.Client);
            Core.LaunchGame(SharedData.Client);
        }

        private void MainGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void DeveloperToolsButton_Click(object sender, RoutedEventArgs e)
        {
            if (App.Busy)
            {
                return;
            }

            App.SwitchPage(new DevToolsPage());
        }

        private void ClientRadio_Checked(object sender, RoutedEventArgs e)
        {
            App.ActiveInstance = SharedData.Client;
            if (App.CurrentPage is IUpdateInstance)
            {
                App.CurrentPage.UpdateInstance();
            }
        }

        private void ServerRadio_Checked(object sender, RoutedEventArgs e)
        {
            App.ActiveInstance = SharedData.Server;
            if (App.CurrentPage is IUpdateInstance)
            {
                App.CurrentPage.UpdateInstance();
            }
        }

        private void PageFrame_ContentRendered(object sender, EventArgs e)
        {
            if (App.CurrentPage is ModPage)
            {
                ApplyForBorder.Visibility = Visibility.Visible;
                SetGrid.Visibility = Visibility.Visible;
            }
            else
            {
                ApplyForBorder.Visibility = Visibility.Hidden;
                SetGrid.Visibility = Visibility.Hidden;
            }
        }

        private void ClientCheck_Click(object sender, RoutedEventArgs e)
        {
            App.ApplyForClient = ClientCheck.IsChecked.Value;
        }

        private void ServerCheck_Click(object sender, RoutedEventArgs e)
        {
            App.ApplyForServer = ServerCheck.IsChecked.Value;
        }

        private void ReadSet_Click(object sender, RoutedEventArgs e)
        {
            if (App.Busy)
            {
                return;
            }

            OpenFileDialog dialog = new OpenFileDialog()
            {
                Filter = "Xml File（*.xml）|*.xml"
            };

            if (dialog.ShowDialog(this).Value)
            {
                XmlHelper.ReadXmlSet(dialog.FileName);
            }
        }

        private void SaveSet_Click(object sender, RoutedEventArgs e)
        {
            if (App.Busy)
            {
                return;
            }

            SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = "Xml File（*.xml）|*.xml"
            };

            if (dialog.ShowDialog(this).Value)
            {
                XmlHelper.WriteXmlSet(dialog.FileName);
            }
        }
    }
}
