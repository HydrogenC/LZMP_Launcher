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
        private Boolean processing = false;
        private static List<String> categoryDict = new List<String>();
        private static Dictionary<String, TreeViewItem> nodeDict = new Dictionary<String, TreeViewItem>();

        public MainWindow()
        {
            InitializeComponent();

            XmlHelper.ReadDefinitions(Shared.WorkingDir + "\\BasicSettings.xml");
            BigTitle.Content = BigTitle.Content.ToString().Replace("%v", Shared.Version);

            Core.CheckAvailability();
            Core.CheckInstallation();
        }

        private void WriteNodes()
        {
            foreach (var i in Shared.Mods)
            {
                if (!MainTree.Items.Contains(i.Value.Category + " Mods"))
                {
                    categoryDict.Add(i.Value.Category + " Mods");
                }

                TreeView.Add
            }

            foreach (var i in categoryDict)
            {
                MainTree.Items.Add(i + " Mods");
            }

            MainTree.ExpandAll();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (!processing)
            {
                Close();
            }
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CleanUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Clean up: This button would delete all unused files in the 'Resources' path. Are you sure to continue? ", "Prompt", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Core.CleanUp();
            }
        }
    }
}
