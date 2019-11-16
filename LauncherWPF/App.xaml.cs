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
        public static Action<bool> BusyAction
        {
            private get;
            set;
        }

        public static Action<dynamic> SwitchPage
        {
            get;
            set;
        }

        private static ModPage mainModPage = null;
        public static ModPage MainModPage
        {
            get
            {
                if (mainModPage == null)
                {
                    mainModPage = new ModPage();
                }
                return mainModPage;
            }
            set => mainModPage = value;
        }

        private static SavesPage mainSavesPage = null;
        public static SavesPage MainSavesPage
        {
            get
            {
                if (mainSavesPage == null)
                {
                    mainSavesPage = new SavesPage();
                }
                return mainSavesPage;
            }
            set => mainSavesPage = value;
        }

        public static dynamic CurrentPage
        {
            get;
            set;
        }

        public static string DefaultTitle
        {
            get;
            set;
        } = "LZMP {0} Launcher";

        public static MinecraftInstance ActiveInstance
        {
            get;
            set;
        }

        public static bool ApplyForClient
        {
            get;
            set;
        }

        public static bool ApplyForServer
        {
            get;
            set;
        }

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
    }
}
