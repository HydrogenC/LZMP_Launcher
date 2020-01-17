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
            App.BeginRename = (Save obj) =>
              {
                  MapFrame.Content = new RenameMapPage(obj);
              };
            App.EndRename = () =>
            {
                MapFrame.Content = App.MainSavesPage;
            };

            XmlHelper.ReadDefinitions(SharedData.WorkingPath + "\\BasicSettings.xml");
            LauncherTitleLabel.Content = SharedData.Title;
            Core.CheckInstallation();
            Core.CheckAvailability();

            App.GeneratePages();
            PageFrame.Content = App.MainModPage;
            MapFrame.Content = App.MainSavesPage;
        }

        private void CloseForm_Click(object sender, RoutedEventArgs e)
        {
            if (!App.Busy)
            {
                Close();
            }
        }

        private void LaunchClientButton_Click(object sender, RoutedEventArgs e)
        {
            Core.ApplyChanges();
            Core.LaunchGame();
        }

        private void MainGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
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

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            App.MainSavesPage.RefreshList();
        }
    }
}
