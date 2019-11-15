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
        public static bool allChecked = false;
        public static Dictionary<string, MainTreeItem> itemDict = new Dictionary<string, MainTreeItem>();
        public static Dictionary<string, MainTreeItem> categoryDict = new Dictionary<string, MainTreeItem>();

        public ModPage()
        {
            InitializeComponent();
            WriteNodes();
            Core.CheckToInstallState(App.CurrentInstance);
        }

        public void UpdateInstance()
        {
            Core.CheckToInstallState(App.CurrentInstance);
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
                CheckAllButton.Content = "Cancel All";
            }
            else
            {
                CheckAllButton.Content = "Check All";
            }
        }

        private void WriteNodes()
        {
            foreach (var i in SharedData.Mods)
            {
                if (!categoryDict.ContainsKey(i.Value.Category))
                {
                    categoryDict[i.Value.Category] = new MainTreeItem(i.Value.Category + " Mods");
                    categoryDict[i.Value.Category].IsCategory = true;
                }

                itemDict[i.Key] = new MainTreeItem(i.Value.Name);
                itemDict[i.Key].IsCategory = false;
                itemDict[i.Key].Parent = categoryDict[i.Value.Category];
                categoryDict[i.Value.Category].Children.Add(itemDict[i.Key]);

                foreach (var j in i.Value.Addons)
                {
                    itemDict[j.Key] = new MainTreeItem(j.Value.Name);
                    itemDict[j.Key].IsCategory = false;
                    itemDict[j.Key].Parent = itemDict[i.Key];
                    itemDict[i.Key].Children.Add(itemDict[j.Key]);
                }
            }

            foreach (var i in categoryDict)
            {
                MainTreeView.Items.Add(i.Value);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            App.SwitchPage(new MenuPage());
        }

        private void CheckAllButton_Click(object sender, RoutedEventArgs e)
        {
            Core.CheckAll(!allChecked);
            CheckIfAllChecked();
        }
    }
}
