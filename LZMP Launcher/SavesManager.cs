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
            if (saves.Length > 0)
            {
                foreach (var i in saves)
                {
                    SavesList.Items.Add(i);
                }
            }
            else
            {
                SavesList.Items.Add(" Empty");
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
            HelpFunctions.CopyDirectory(save.Dir, tmpDir + "save\\");
            XmlHelper.WriteXmlSet(tmpDir + "Set.xml", false);

            if (Shared.mods["ctk"].Node.Checked && Directory.Exists(Shared.scriptDir))
            {
                HelpFunctions.CopyDirectory(Shared.scriptDir, tmpDir + "scripts\\");
            }

            if (Directory.Exists(Shared.jmDataDir))
            {
                HelpFunctions.CopyDirectory(Shared.jmDataDir + save.LevelName, tmpDir + "jm\\");
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
            String tmpDir = Shared.workingDir + "\\Tmp\\";
            Directory.CreateDirectory(tmpDir);

            String zipName = zipFile.Substring(zipFile.LastIndexOf('\\') + 1);
            zipName = zipName.Substring(0, zipName.Length - 4);
            FastZip zip = new FastZip();
            zip.ExtractZip(zipFile, tmpDir, null);

            Directory.CreateDirectory(Shared.saveDir + zipName);
            HelpFunctions.CopyDirectory(tmpDir + "save", Shared.saveDir + zipName);

            if (MessageBox.Show("Override the current modset with the save's? ", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                XmlHelper.ReadXmlSet(tmpDir + "Set.xml", false);
                Mod[] applyList = MainForm.GenerateApplyList();
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
                    File.Copy(tmpDir + "Set.xml", XmlDialog.FileName);
                }
            }

            if (Directory.Exists(tmpDir + "scripts\\"))
            {
                HelpFunctions.CopyDirectory(tmpDir + "scripts\\", Shared.scriptDir);
            }

            if (Directory.Exists(tmpDir + "jm\\"))
            {
                // Directory.CreateDirectory(Shared.jmDataDir);
            }

            processing = false;
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            RefreshList();
        }
    }
}
