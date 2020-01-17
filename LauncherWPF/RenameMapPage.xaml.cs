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
    /// RenameMapPage.xaml 的交互逻辑
    /// </summary>
    public partial class RenameMapPage : Page
    {
        private Save save;

        public RenameMapPage(Save save)
        {
            InitializeComponent();
            this.save = save;
            MapTitleLabel.Content = save.DisplayName;
            LevelBox.Text = save.LevelName;
            FolderBox.Text = save.FolderName;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (save.LevelName != LevelBox.Text)
                {
                    save.LevelName = LevelBox.Text;
                }

                if (save.FolderName != FolderBox.Text)
                {
                    save.FolderName = FolderBox.Text;
                }
            }
            catch (System.IO.IOException)
            {
                MessageBox.Show("Please notice that changing the folder name of a server map is invalid. ", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("An unexpected exception happened, maybe the map is broken. ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            App.MainSavesPage.RefreshList();
            App.EndRename();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            App.EndRename();
        }
    }
}
