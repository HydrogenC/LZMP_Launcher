﻿using System;
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
            get;
            set;
        }

        public static Action<dynamic> SwitchPage
        {
            get;
            set;
        }

        private static dynamic mainModPage = null;
        public static dynamic MainModPage
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

        public static dynamic CurrentPage
        {
            get;
            set;
        }

        public static MinecraftInstance CurrentInstance
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
    }
}
