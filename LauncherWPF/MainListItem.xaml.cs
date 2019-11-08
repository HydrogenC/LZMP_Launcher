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

namespace LauncherWPF
{
    /// <summary>
    /// MainListItem.xaml 的交互逻辑
    /// </summary>
    public partial class MainListItem : UserControl
    {
        public MainListItem()
        {
            InitializeComponent();
        }

        public string Title
        {
            get => (string)TitleLabel.Content;
            set => TitleLabel.Content = value;
        }

        public string Description
        {
            get => (string)DescriptionLabel.Content;
            set => DescriptionLabel.Content = value;
        }
    }
}
