using LauncherCore;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LauncherUI
{
    public partial class MapRenameForm : Form
    {
        private Save currentSave;

        #region Drag
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;

        public void Form_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }
        #endregion

        public MapRenameForm(ref Save save)
        {
            InitializeComponent();

            currentSave = save;
            FolderBox.Text = save.FolderName;
            LevelBox.Text = save.LevelName;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentSave.LevelName != LevelBox.Text)
                {
                    currentSave.LevelName = LevelBox.Text;
                }

                if (currentSave.FolderName != FolderBox.Text)
                {
                    currentSave.FolderName = FolderBox.Text;
                }
            }
            catch (PlatformNotSupportedException)
            {
                MessageBox.Show("Please notice that changing the folder name of a server map is invalid. ", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("An unexpected exception happened, maybe the map is broken. ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Close();
        }
    }
}
