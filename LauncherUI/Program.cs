using System;
using System.Windows.Forms;

namespace LauncherUI
{
    public static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm form = new MainForm();
            LauncherCore.SharedData.MainWindow = form;
            Application.Run(form);
        }
    }
}
