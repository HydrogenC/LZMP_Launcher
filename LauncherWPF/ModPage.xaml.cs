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
    /// ModPage.xaml 的交互逻辑
    /// </summary>
    public partial class ModPage : Page
    {
        private Action<Page> action;
        private Dictionary<string, TreeViewItem> itemDict = new Dictionary<string, TreeViewItem>();
        private Dictionary<string, TreeViewItem> categoryDict = new Dictionary<string, TreeViewItem>();

        public ModPage(Action<Page> action)
        {
            InitializeComponent();
            this.action = action;

            WriteNodes();
        }

        private void WriteNodes()
        {
            foreach (var i in SharedData.Mods)
            {
                if (!categoryDict.ContainsKey(i.Value.Category))
                {
                    categoryDict[i.Value.Category] = new TreeViewItem();
                    categoryDict[i.Value.Category].Header = i.Value.Category + " Mods";
                    MainTreeView.Items.Add(categoryDict[i.Value.Category]);
                }

                itemDict[i.Key] = new TreeViewItem();
                var checkBox = new CheckBox();
                checkBox.Content = i.Value.Name;
                itemDict[i.Key].Header = checkBox;
                categoryDict[i.Value.Category].Items.Add(itemDict[i.Key]);

                foreach (var j in i.Value.Addons)
                {
                    itemDict[j.Key] = new TreeViewItem();
                    var addonCheckBox = new CheckBox();
                    addonCheckBox.Content = j.Value.Name;
                    itemDict[i.Key].Items.Add(itemDict[j.Key]);
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            action(new MenuPage(action));
        }
    }
}
