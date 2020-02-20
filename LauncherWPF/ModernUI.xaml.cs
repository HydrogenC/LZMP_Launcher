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
using LauncherCore;

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

        public ModernUI()
        {
            InitializeComponent();
        }

        private void LaunchButton_Click(object sender, RoutedEventArgs e)
        {
            Core.ApplyChanges();
            Core.LaunchGame();
        }
    }
}
