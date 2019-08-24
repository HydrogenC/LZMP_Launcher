using LauncherCore;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LauncherUI
{
    public partial class SavesManager : Form
    {
        private Boolean processing = false;

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
            CheckForIllegalCrossThreadCalls = false;
            RefreshList();
        }

        private void RefreshList()
        {
            Save[] saves = SavesHelper.GetSaves();

            SavesList.Items.Clear();
            foreach (var i in saves)
            {
                SavesList.Items.Add(i);
            }
        }

        private void ExitForm_Click(object sender, EventArgs e)
        {
            if (!processing)
            {
                Close();
            }
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            Save selection = SavesList.SelectedItem as Save;
            if (selection == null)
            {
                MessageBox.Show("Please select a map in the list to export. ", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ExportDialog.FileName = selection.LevelName + ".zip";
            if (ExportDialog.ShowDialog() == DialogResult.OK)
            {
                Action<Save, String> action = new Action<Save, String>(SavesHelper.ExportSave);
                processing = true;
                action.BeginInvoke(selection, ExportDialog.FileName, UniversalAsyncCallback, null); ;

                String prevText = "";
                while (processing)
                {
                    if (SavesStatus.status != prevText)
                    {
                        BigTitle.Text = SavesStatus.status + "...";
                        prevText = SavesStatus.status;
                    }

                    Application.DoEvents();
                }

                BigTitle.Text = "Saves Manager";
                MessageBox.Show("Finished! ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void UniversalAsyncCallback(IAsyncResult ar)
        {
            processing = false;
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            if (OpenDialog.ShowDialog() == DialogResult.OK)
            {
                Action<String> action = new Action<String>(SavesHelper.ImportSave);
                processing = true;
                action.BeginInvoke(OpenDialog.FileName, UniversalAsyncCallback, null);

                String prevText = "";
                while (processing)
                {
                    if (SavesStatus.status != prevText)
                    {
                        BigTitle.Text = SavesStatus.status + "...";
                        prevText = SavesStatus.status;
                    }

                    Application.DoEvents();
                }

                BigTitle.Text = "Saves Manager";
                RefreshList();
                MessageBox.Show("Finished! ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            RefreshList();
        }
    }
}
