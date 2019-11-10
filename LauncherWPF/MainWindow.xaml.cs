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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
                MessageBoxButton btn;
                MessageBoxImage icn;
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
                    default:
                        btn = MessageBoxButton.OK;
                        icn = MessageBoxImage.None;
                        break;
                }
                MessageBoxResult rst = MessageBox.Show(this, content, caption, btn, icn);
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
            SharedData.BrowzeFile = (string initial, string filter, string caption) =>
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

                bool? succeed = dialog.ShowDialog(this);
                if (succeed.HasValue ? succeed.Value : false)
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
                PageFrame.Content = page;
                App.CurrentPage = page.GetType();
            };
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            Mod.GetToInstallState = (Mod mod) => ModPage.itemDict[mod.Key].Checked == CheckBoxState.Checked;
            Mod.SetToInstallState = (Mod mod, bool flag) =>
            {
                if (flag)
                {
                    ModPage.itemDict[mod.Key].Checked = CheckBoxState.Checked;
                }
                else
                {
                    ModPage.itemDict[mod.Key].Checked = CheckBoxState.NotChecked;
                }
            };

            XmlHelper.ReadDefinitions(MinecraftInstance.WorkingPath + "\\BasicSettings.xml");
            LauncherTitleLabel.Content = string.Format((string)LauncherTitleLabel.Content, SharedData.Version);
            Core.CheckInstallation();

            if (System.IO.Directory.Exists(MinecraftInstance.WorkingPath + "\\Mods"))
            {
                Core.CopyDirectory(MinecraftInstance.WorkingPath + "\\Mods", SharedData.Client.ModPath);
                Core.CopyDirectory(MinecraftInstance.WorkingPath + "\\Mods", SharedData.Server.ModPath);
            }
            Core.CheckAvailability();
            ClientRadio.IsChecked = true;

            PageFrame.Content = new MenuPage();
            App.CurrentPage = typeof(MenuPage);
        }

        private void CloseForm_Click(object sender, RoutedEventArgs e)
        {
            Close();
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

        }

        private void ClientRadio_Checked(object sender, RoutedEventArgs e)
        {
            App.CurrentInstance = SharedData.Client;
        }

        private void PageFrame_ContentRendered(object sender, EventArgs e)
        {
            if (App.CurrentPage == typeof(ModPage))
            {
                ApplyForBorder.Visibility = Visibility.Visible;
            }
            else
            {
                ApplyForBorder.Visibility = Visibility.Hidden;
            }
        }

        private void ServerRadio_Checked(object sender, RoutedEventArgs e)
        {
            App.CurrentInstance = SharedData.Server;
        }

        private void ClientCheck_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
