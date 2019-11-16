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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LauncherCore;

namespace LauncherWPF
{
    /// <summary>
    /// DevToolsPage.xaml 的交互逻辑
    /// </summary>
    public partial class DevToolsPage : Page
    {
        public DevToolsPage()
        {
            InitializeComponent();
        }

        private void CleanUp_Click(object sender, RoutedEventArgs e)
        {
            App.Busy = true;
            Core.CleanUp();
            App.Busy = false;
        }

        private void Initialize_Click(object sender, RoutedEventArgs e)
        {
            App.Busy = true;

            foreach (var i in SharedData.Mods)
            {
                i.Value.ToInstall = false;
            }
            Core.ApplyChanges(SharedData.Client);

            Core.CopyDirectory(SharedData.Client.ModPath, MinecraftInstance.WorkingPath + "\\Mods");

            if (Directory.Exists(SharedData.Client.ModPath))
            {
                Directory.Delete(SharedData.Client.ModPath, true);
            }

            if (Directory.Exists(SharedData.Server.ModPath))
            {
                Directory.Delete(SharedData.Server.ModPath, true);
            }

            Core.CheckToInstallState(SharedData.Client);
            Core.CheckToInstallState(SharedData.Server);
            App.Busy = false;

            App.MainModPage = new RestartRequest();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            App.SwitchPage(new MenuPage());
        }
    }
}
