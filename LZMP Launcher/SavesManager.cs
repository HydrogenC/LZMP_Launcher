using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;

namespace LZMP_Launcher
{
    public partial class SavesManager : Form
    {
        #region Drag
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;

        private void SavesManager_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }
        #endregion

        public SavesManager()
        {
            InitializeComponent();

            Save[] saves = GetSaves();
            if (saves.Length > 0)
            {
                SavesList.Items.Clear();

                foreach (var i in saves)
                {
                    SavesList.Items.Add(i);
                }
            }
        }

        private void ExitForm_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {

        }

        private Save[] GetSaves()
        {
            List<Save> saves = new List<Save>();
            foreach(String i in Directory.EnumerateDirectories(Shared.savesDir))
            {
                if (File.Exists(i + "\\level.dat"))
                {
                    saves.Add(new Save(i));
                }
            }
            return saves.ToArray();
        }
    }
}
