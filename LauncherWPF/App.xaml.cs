using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using LauncherCore;

namespace LauncherWPF
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static Action<Save> BeginRename
        {
            get;
            set;
        }

        public static Action EndRename
        {
            get;
            set;
        }

        /*
        public static Action<bool> BusyAction
        {
            private get;
            set;
        }

        public static ModPage MainModPage
        {
            get;
            set;
        } = null;

        public static SavesPage MainSavesPage
        {
            get;
            set;
        } = null;

        private static bool busy = false;
        public static bool Busy
        {
            get => busy;
            set
            {
                BusyAction(value);
                busy = value;
            }
        }

        public static Action<string> SetTitleText
        {
            private get;
            set;
        }

        public static Func<string> GetTitleText
        {
            private get;
            set;
        }

        public static string TitleText
        {
            get => GetTitleText();
            set => SetTitleText(value);
        }

        public static void GeneratePages()
        {
            if (MainModPage == null)
            {
                MainModPage = new ModPage();
                Core.CheckToInstallState();
            }
            if (MainSavesPage == null)
            {
                MainSavesPage = new SavesPage();
                MainSavesPage.RefreshList();
            }
        }
        */
    }
}
