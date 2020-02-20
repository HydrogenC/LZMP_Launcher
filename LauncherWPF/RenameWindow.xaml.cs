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
    /// RenameMapWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RenameWindow : Window
    {
        private IEditable editable;

        public RenameWindow(IEditable save)
        {
            InitializeComponent();
            editable = save;
            MapTitleLabel.Content = save.ToString();
            if (editable is Save)
            {
                LevelBox.Text = (editable as Save).LevelName;
                FolderBox.Text = (editable as Save).FolderName;
                return;
            }

            if (editable is Modset)
            {
                LevelNameLabel.Visibility = Visibility.Hidden;
                LevelBox.Visibility = Visibility.Hidden;
                FolderNameLabel.Content = "Modset name: ";
                LevelBox.Text = "";
                FolderBox.Text = editable.ToString();
            }

        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {

#if !DEBUG
            try
            {
#endif

            if (!string.IsNullOrEmpty(FolderBox.Text))
            {
                editable.Rename(FolderBox.Text, true);
            }

            if (!string.IsNullOrEmpty(LevelBox.Text))
            {
                editable.Rename(LevelBox.Text, false);
            }

#if !DEBUG
            }
            catch (Exception)
            {
                MessageBox.Show("An unexpected exception happened, maybe the map or modset is broken. ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
#endif

            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
