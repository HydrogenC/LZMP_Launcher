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
            RefreshList();
        }

        private void RefreshList()
        {
            Save[] saves = GetSaves();
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
            ExportDialog.FileName = selection.LevelName + ".zip";
            if (ExportDialog.ShowDialog() == DialogResult.OK)
            {
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
            Helper.CopyDirectory(save.Dir, tmpDir + "save\\");
            XmlHelper.WriteXmlSet(tmpDir + "Set.xml", false);

            if (Shared.mods["ctk"].Node.Checked && Directory.Exists(Shared.scriptDir))
            {
                Helper.CopyDirectory(Shared.scriptDir, tmpDir + "scripts\\");
            }

            if (Directory.Exists(Shared.jmDataDir + save.LevelName))
            {
                Helper.CopyDirectory(Shared.jmDataDir + save.LevelName, tmpDir + "jm\\");
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

        private void ImportButton_Click(object sender, EventArgs e)
        {
            if (OpenDialog.ShowDialog() == DialogResult.OK)
            {
                Action<String> action = new Action<String>(ImportSave);
                action.BeginInvoke(OpenDialog.FileName, null, null);
                processing = true;

                while (processing)
                {
                    Application.DoEvents();
                }

                BigTitle.Text = "Saves Manager";
                RefreshList();
                MessageBox.Show("Finished! ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ImportSave(String zipFile)
        {
            BigTitle.Text = "Decompressing...";

            String tmpDir = Shared.workingDir + "\\Tmp\\";
            Directory.CreateDirectory(tmpDir);

            String zipName = zipFile.Substring(zipFile.LastIndexOf('\\') + 1);
            zipName = zipName.Substring(0, zipName.Length - 4);
            FastZip zip = new FastZip();
            zip.ExtractZip(zipFile, tmpDir, null);

            BigTitle.Text = "Importing Map...";

            Save save = new Save(tmpDir + "save");
            Helper.CopyDirectory(tmpDir + "save", Shared.saveDir + zipName);

            if (MessageBox.Show("Override the current modset with the map's? If you choose No, you can select where to save the map's modset later. ", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                XmlHelper.ReadXmlSet(tmpDir + "Set.xml", false);
                Mod[] applyList = Helper.GenerateApplyList();
                foreach (var i in applyList)
                {
                    if (i.Installed)
                    {
                        i.Uninstall();
                    }
                    else
                    {
                        i.Install();
                    }
                }
            }
            else
            {
                if (XmlDialog.ShowDialog() == DialogResult.OK)
                {
                    File.Copy(tmpDir + "Set.xml", XmlDialog.FileName, true);
                }
            }

            if (Directory.Exists(tmpDir + "scripts\\"))
            {
                Helper.CopyDirectory(tmpDir + "scripts\\", Shared.scriptDir);
            }

            if (Directory.Exists(tmpDir + "jm\\"))
            {
                Helper.CopyDirectory(tmpDir + "jm\\", Shared.jmDataDir + save.LevelName);
            }

            BigTitle.Text = "Cleaning up...";

            Directory.Delete(tmpDir, true);
            processing = false;
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            RefreshList();
        }
    }
}
