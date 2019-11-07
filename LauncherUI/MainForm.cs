using LauncherCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LauncherUI
{
    public partial class MainForm : Form
    {
        private Boolean locked = false, processing = false, allChecked = false, promptOnExit = false;
        private static Dictionary<String, TreeNode> nodeDict = new Dictionary<String, TreeNode>();
        private static Dictionary<String, TreeNode> categoryDict = new Dictionary<String, TreeNode>();
        private static MinecraftInstance activeInstance;

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

        public MainForm()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;
            XmlHelper.ReadDefinitions(MinecraftInstance.WorkingPath + "\\BasicSettings.xml");
            BigTitle.Text += SharedData.Version;
            Core.CheckInstallation();
            WriteNodes();

            activeInstance = SharedData.Client;
            Mod.GetToInstallState = GetNodeChecked;
            Mod.SetToInstallState = SetNodeChecked;
            ClientCheckBox.Checked = true;
            ServerCheckBox.Checked = true;
            ClientRadioButton.Checked = true;
            if (Directory.Exists(MinecraftInstance.WorkingPath + "\\Mods"))
            {
                Core.CopyDirectory(MinecraftInstance.WorkingPath + "\\Mods", SharedData.Client.ModPath);
                Core.CopyDirectory(MinecraftInstance.WorkingPath + "\\Mods", SharedData.Server.ModPath);
            }

            Core.CheckAvailability();
            CheckIfAllChecked();
            promptOnExit = false;
            SaveXmlDialog.InitialDirectory = MinecraftInstance.WorkingPath + "\\Sets\\";
        }

        private void ResetSmallTitle()
        {
            SmallTitle.Text = "ExMatics";
        }

        private void RefreshList(MinecraftInstance instance)
        {
            Save[] saves = SavesHelper.GetSaves(instance);

            SavesList.Items.Clear();
            foreach (var i in saves)
            {
                SavesList.Items.Add(i);
            }
        }

        private void SetInstance(MinecraftInstance instance)
        {
            activeInstance = instance;
            Core.CheckToInstallState(instance);
            CheckIfAllChecked();
            RefreshList(instance);
        }

        private static Boolean GetNodeChecked(Mod mod)
        {
            return nodeDict[mod.Key].Checked;
        }

        private static void SetNodeChecked(Mod mod, Boolean flag)
        {
            if (nodeDict[mod.Key].Checked != flag)
            {
                nodeDict[mod.Key].Checked = flag;
            }
        }

        private void WriteNodes()
        {
            foreach (var i in SharedData.Mods)
            {
                if (!categoryDict.ContainsKey(i.Value.Category))
                {
                    categoryDict[i.Value.Category] = new TreeNode(i.Value.Category + " Mods");
                }

                nodeDict[i.Key] = new TreeNode(i.Value.Name);
                categoryDict[i.Value.Category].Nodes.Add(nodeDict[i.Key]);

                foreach (var j in i.Value.Addons)
                {
                    nodeDict[j.Key] = new TreeNode(j.Value.Name);
                    nodeDict[i.Key].Nodes.Add(nodeDict[j.Key]);
                }
            }

            foreach (var i in categoryDict)
            {
                MainTree.Nodes.Add(i.Value);
            }

            MainTree.ExpandAll();
        }

        private void CheckIfAllChecked()
        {
            allChecked = true;
            foreach (var i in SharedData.Mods)
            {
                if (!i.Value.ToInstall)
                {
                    allChecked = false;
                }

                foreach (var j in i.Value.Addons)
                {
                    if (!j.Value.ToInstall)
                    {
                        allChecked = false;
                        break;
                    }
                }

                if (!allChecked)
                {
                    break;
                }
            }

            if (allChecked)
            {
                ToggleCheck.Text = "Cancel All";
            }
            else
            {
                ToggleCheck.Text = "Check All";
            }
        }

        private void MainTree_AfterCheck(object sender, TreeViewEventArgs e)
        {
            TreeNode i = e.Node;
            promptOnExit = true;

            if (!i.Checked)
            {
                for (int j = 0; j < i.GetNodeCount(false); j += 1)
                {
                    if (i.Nodes[j].Checked)
                    {
                        i.Nodes[j].Checked = false;
                    }
                }
            }
            else
            {
                if (i.Level > 0)
                {
                    if (!i.Parent.Checked)
                    {
                        i.Parent.Checked = true;
                    }
                }
            }

            if (i.Checked)
            {
                CheckIfAllChecked();
            }
        }

        #region Buttons
        private void ExitForm_Click(object sender, EventArgs e)
        {
            if (!processing)
            {
                Application.Exit();
            }
        }

        private void ToggleCheck_Click(object sender, EventArgs e)
        {
            Core.CheckAll(!allChecked);
            CheckIfAllChecked();
        }

        private void LaunchClientButton_Click(object sender, EventArgs e)
        {
            Core.ApplyChanges(SharedData.Client);
            Core.LaunchGame(SharedData.Client);
        }

        private void SaveSet_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(SaveXmlDialog.InitialDirectory))
            {
                Directory.CreateDirectory(SaveXmlDialog.InitialDirectory);
            }
            if (SaveXmlDialog.ShowDialog() == DialogResult.OK)
            {
                XmlHelper.WriteXmlSet(SaveXmlDialog.FileName);
            }
        }

        private void ReadSet_Click(object sender, EventArgs e)
        {
            if (OpenXmlDialog.ShowDialog() == DialogResult.OK)
            {
                XmlHelper.ReadXmlSet(OpenXmlDialog.FileName);
                promptOnExit = true;
            }
        }

        private void Apply_Click(object sender, EventArgs e)
        {
            if (locked)
            {
                return;
            }

            SmallTitle.Text = "Applying";
            locked = true;

            try
            {
                Action<MinecraftInstance> action = new Action<MinecraftInstance>(Core.ApplyChanges);
                if (ClientCheckBox.Checked)
                {
                    processing = true;
                    action.BeginInvoke(SharedData.Client, ProcessEndCallback, null);

                    while (processing)
                    {
                        CurrentProgress.Update(SmallTitle);
                        Application.DoEvents();
                    }

                    Core.CheckToInstallState(SharedData.Client);
                }

                if (ServerCheckBox.Checked)
                {
                    processing = true;
                    action.BeginInvoke(SharedData.Server, ProcessEndCallback, null);

                    while (processing)
                    {
                        CurrentProgress.Update(SmallTitle);
                        Application.DoEvents();
                    }

                    Core.CheckToInstallState(SharedData.Server);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Unknown exception caught! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ResetSmallTitle();
                promptOnExit = false;
                locked = false;
            }

            MessageBox.Show("Finished! ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CleanUpButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Clean up: This button would delete all unused files in the 'Resources' path. Are you sure to continue? ", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Cursor = Cursors.WaitCursor;
                Core.CleanUp();
                Cursor = Cursors.Default;
            }
        }

        private void LaunchServerButton_Click(object sender, EventArgs e)
        {
            Core.ApplyChanges(SharedData.Server);
            Core.LaunchGame(SharedData.Server);
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            if (locked)
            {
                return;
            }

            if (ImportDialog.ShowDialog() == DialogResult.OK)
            {
                locked = true;

                try
                {
                    Action<String, MinecraftInstance> action = new Action<String, MinecraftInstance>(SavesHelper.ImportSave);
                    processing = true;
                    action.BeginInvoke(ImportDialog.FileName, activeInstance, ProcessEndCallback, null);

                    String prevText = "";
                    while (processing)
                    {
                        if (CurrentProgress.status != prevText)
                        {
                            SmallTitle.Text = CurrentProgress.status + "...";
                            prevText = CurrentProgress.status;
                        }

                        Application.DoEvents();
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Unknown exception caught! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    ResetSmallTitle();
                    RefreshList(activeInstance);
                    locked = false;
                }

                MessageBox.Show("Finished! ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            RefreshList(activeInstance);
        }

        private void InitializeButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Initialize: This button would reset the modpack to the uninstalled state. Are you sure to continue? ", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Cursor = Cursors.WaitCursor;
                foreach (var i in SharedData.Mods)
                {
                    i.Value.ToInstall = false;
                }
                Core.ApplyChanges(SharedData.Client);

                Core.CopyDirectory(SharedData.Client.ModPath, MinecraftInstance.WorkingPath + "\\Mods");

                if (Directory.Exists(SharedData.Client.ModPath))
                {
                    Directory.Delete(SharedData.Client.ModPath, true);
                }

                if (Directory.Exists(SharedData.Server.ModPath))
                {
                    Directory.Delete(SharedData.Server.ModPath, true);
                }

                Core.CheckToInstallState(SharedData.Client);
                Core.CheckToInstallState(SharedData.Server);
                Apply.Enabled = false;

                Cursor = Cursors.Default;
            }
        }
        #endregion

        private void ProcessEndCallback(IAsyncResult ar)
        {
            processing = false;
        }

        private void ExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (locked)
            {
                return;
            }

            Save selection = SavesList.SelectedItem as Save;
            if (selection == null)
            {
                MessageBox.Show("Please select a map in the list to export. ", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                ExportDialog.FileName = selection.LevelName + ".zip";
                if (ExportDialog.ShowDialog() == DialogResult.OK)
                {
                    locked = true;

                    try
                    {
                        Action<Save, String> action = new Action<Save, String>(SavesHelper.ExportSave);
                        processing = true;
                        action.BeginInvoke(selection, ExportDialog.FileName, ProcessEndCallback, null); ;

                        while (processing)
                        {
                            CurrentProgress.Update(SmallTitle);
                            Application.DoEvents();
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Unknown exception caught! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        ResetSmallTitle();
                        locked = false;
                    }

                    MessageBox.Show("Finished! ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void RenameMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save selection = SavesList.SelectedItem as Save;
            if (selection == null)
            {
                MessageBox.Show("Please select a map in the list to rename. ", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MapRenameForm form = new MapRenameForm(ref selection);
                form.ShowDialog();
                RefreshList(activeInstance);
            }
        }

        private void DeleteMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save selection = SavesList.SelectedItem as Save;
            if (selection == null)
            {
                MessageBox.Show("Please select a map in the list to delete. ", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (MessageBox.Show("Are you sure to delete? You cannot revert this! ", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    selection.Delete();
                    RefreshList(activeInstance);
                }
            }
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (ClientRadioButton.Checked)
            {
                SetInstance(SharedData.Client);
                ServerRadioButton.Checked = false;
                return;
            }
            if (ServerRadioButton.Checked)
            {
                SetInstance(SharedData.Server);
                ClientRadioButton.Checked = false;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (promptOnExit)
            {
                DialogResult result = MessageBox.Show("Are you sure to discard the unapplied changes? ", "Prompt", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                switch (result)
                {
                    case DialogResult.Yes:
                        e.Cancel = false;
                        break;
                    case DialogResult.No:
                        Apply_Click(null, null);
                        e.Cancel = false;
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                    default:
                        e.Cancel = false;
                        break;
                }
            }
        }
    }
}
