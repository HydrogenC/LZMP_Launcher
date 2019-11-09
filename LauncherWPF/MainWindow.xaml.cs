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

namespace LauncherWPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public void SetFrameContent(Page content)
        {
            PageFrame.Content = content;
        }

        public MainWindow()
        {
            InitializeComponent();
            SharedData.DisplayMessage = (string content, string caption, MessageBoxButton btn, MessageBoxImage img) => MessageBoxResult
            {

            }

            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

            XmlHelper.ReadDefinitions(MinecraftInstance.WorkingPath + "\\BasicSettings.xml");
            LauncherTitleLabel.Content = string.Format((string)LauncherTitleLabel.Content, SharedData.Version);
            Core.CheckInstallation();

            PageFrame.Content = new MenuPage(new Action<Page>(SetFrameContent));
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
    }
}
