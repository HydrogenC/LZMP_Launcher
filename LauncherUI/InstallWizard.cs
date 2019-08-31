using LauncherCore;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LauncherUI
{
    public partial class InstallWizard : Form
    {
        public static String status = "";
        public static Boolean processing = false;

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

        public InstallWizard()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private static void InstallInstance(Boolean installClient, Boolean installServer)
        {
            String sourcePath = MinecraftInstance.WorkingPath + "\\Mods";
            if (installClient)
            {
                status = "Installing client";
                Core.CopyDirectory(sourcePath, SharedData.Client.ModPath);
            }
            if (installServer)
            {
                status = "Installing server";
                Core.CopyDirectory(sourcePath, SharedData.Server.ModPath);
            }

            status = "Cleaning up";
            Directory.Delete(sourcePath, true);

            status = "Completed";
            processing = false;
        }

        private void InstallLabel_Click(object sender, EventArgs e)
        {
            if ((!ClientCheckBox.Checked) && (!ServerCheckBox.Checked))
            {
                MessageBox.Show("Please select at least one of the instances to install. ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Action<Boolean, Boolean> action = new Action<Boolean, Boolean>(InstallInstance);
            processing = true;
            action.BeginInvoke(ClientCheckBox.Checked, ServerCheckBox.Checked, null, null);

            String prevText = "";
            while (processing)
            {
                if (status != prevText)
                {
                    TitleLabel.Text = status + "...";
                    prevText = status;
                }

                Application.DoEvents();
            }
            Close();
        }
    }
}
