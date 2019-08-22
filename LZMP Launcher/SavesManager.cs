using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LZMP_Launcher
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
            if (!processing)
            {
                Close();
            }
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            if (ExportDialog.ShowDialog() == DialogResult.OK)
            {
                Save selection = SavesList.SelectedItem as Save;
                Action<Save> action = new Action<Save>(ExportSave);
                action.BeginInvoke(selection, null, null);
                processing = true;

                while (processing)
                {
                    Application.DoEvents();
                }

                BigTitle.Text = "Saves Manager";
                MessageBox.Show("Finished! ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ExportSave(Save save)
        {
            BigTitle.Text = "Preparing...";

            String tmpDir = Shared.workingDir + "\\Tmp\\";
            Directory.CreateDirectory(tmpDir);
            HelpFunctions.CopyDirectory(save.Dir, tmpDir + "\\save\\");
            XmlHelper.WriteXmlSet(tmpDir + "\\Set.xml", false);

            if (Shared.mods["ctk"].Node.Checked && Directory.Exists(Shared.scriptDir))
            {
                HelpFunctions.CopyDirectory(Shared.scriptDir, tmpDir + "\\scripts\\");
            }

            if (Directory.Exists(Shared.jmDataDir))
            {
                HelpFunctions.CopyDirectory(Shared.jmDataDir + save.LevelName, tmpDir + "\\jm\\");
            }

            BigTitle.Text = "Compressing...";

            FastZip zip = new FastZip();
            zip.CreateZip(ExportDialog.FileName, tmpDir, true, null);

            BigTitle.Text = "Cleaning up...";

            Directory.Delete(tmpDir, true);
            processing = false;
        }

        private Save[] GetSaves()
        {
            List<Save> saves = new List<Save>();
            foreach (String i in Directory.GetDirectories(Shared.saveDir))
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
