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
using Microsoft.Win32;

namespace LauncherWPF
{
    /// <summary>
    /// SavesPage.xaml 的交互逻辑
    /// </summary>
    public partial class SavesPage : Page, IUpdateInstance
    {
        private bool processing = false;

        public SavesPage()
        {
            InitializeComponent();
        }

        private void ProcessEndCallback(IAsyncResult ar)
        {
            processing = false;
        }

        public void UpdateInstance()
        {
            MainListBox.Items.Clear();

            Save[] saves = SavesHelper.GetSaves(App.ActiveInstance);
            foreach (var i in saves)
            {
                MainListBox.Items.Add(i);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (App.Busy)
            {
                return;
            }

            App.SwitchPage(new MenuPage());
        }

        private void RenameButton_Click(object sender, RoutedEventArgs e)
        {
            if (App.Busy)
            {
                return;
            }

            Save selection = MainListBox.SelectedItem as Save;
            if (selection == null)
            {
                MessageBox.Show("Please select a map in the list to rename. ", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                App.SwitchPage(new RenameMapPage(selection));
            }
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            if (App.Busy)
            {
                return;
            }

            OpenFileDialog dialog = new OpenFileDialog()
            {
                Filter = "Zip File（*.zip）| *.zip"
            };

            if (dialog.ShowDialog().Value)
            {
                App.Busy = true;

                try
                {
                    processing = true;
                    SavesHelper.ImportSave.BeginInvoke(dialog.FileName, App.ActiveInstance, ProcessEndCallback, null);

                    string prevText = string.Empty;
                    while (processing)
                    {
                        if (CurrentProgress.status != prevText)
                        {
                            App.TitleText = CurrentProgress.status + "...";
                            prevText = CurrentProgress.status;
                        }

                        DispatcherHelper.DoEvents();
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Unknown exception caught! ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    UpdateInstance();
                    App.Busy = false;
                    App.TitleText = App.DefaultTitle;
                }

                MessageBox.Show("Finished! ", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            if (App.Busy)
            {
                return;
            }

            Save selection = MainListBox.SelectedItem as Save;
            if (selection == null)
            {
                MessageBox.Show("Please select a map in the list to export. ", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                SaveFileDialog dialog = new SaveFileDialog()
                {
                    Filter = "Zip File（*.zip）| *.zip",
                    FileName = selection.LevelName + ".zip"
                };
                if (dialog.ShowDialog().Value)
                {
                    App.Busy = true;

                    try
                    {
                        processing = true;
                        SavesHelper.ExportSave.BeginInvoke(selection, dialog.FileName, ProcessEndCallback, null);

                        while (processing)
                        {
                            if (App.TitleText != CurrentProgress.status)
                            {
                                App.TitleText = CurrentProgress.status;
                            }
                            DispatcherHelper.DoEvents();
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Unknown exception caught! ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    finally
                    {
                        App.Busy = false;
                        App.TitleText = App.DefaultTitle;
                    }

                    MessageBox.Show("Finished! ", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (App.Busy)
            {
                return;
            }

            Save selection = MainListBox.SelectedItem as Save;
            if (selection == null)
            {
                MessageBox.Show("Please select a map in the list to delete. ", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                if (MessageBox.Show("Are you sure to delete? You cannot revert this! ", "Prompt", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    selection.Delete();
                    UpdateInstance();
                }
            }
        }
    }
}
