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
        private Boolean processing = false, allChecked = false, promptOnExit = false;
        private static Dictionary<String, TreeNode> nodeDict = new Dictionary<String, TreeNode>();
        private static Dictionary<String, TreeNode> categoryDict = new Dictionary<String, TreeNode>();

        #region Drag
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }
        #endregion

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

        public MainForm()
        {
            InitializeComponent();

            Mod.GetToInstallState = GetNodeChecked;
            Mod.SetToInstallState = SetNodeChecked;

            CheckForIllegalCrossThreadCalls = false;
            MainProgressBar.Visible = false;
            BigTitle.Visible = true;

            XmlHelper.ReadDefinitions(Shared.WorkingDir + "\\BasicSettings.xml");
            BigTitle.Text += Shared.Version;
            WriteNodes();

            Core.CheckAvailability();
            Core.CheckInstallation();
            CheckIfAllChecked();
            promptOnExit = false;
            SaveDialog.InitialDirectory = Shared.WorkingDir + "\\Sets\\";
        }

        private void WriteNodes()
        {
            foreach (var i in Shared.Mods)
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
            foreach (var i in Shared.Mods)
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
        }

        private void LaunchButton_Click(object sender, EventArgs e)
        {
            Apply_Click(null, null);
            LauncherCore.Core.LaunchGame();
        }

        private void SaveSet_Click(object sender, EventArgs e)
        {
            Apply_Click(null, null);
            if (!Directory.Exists(SaveDialog.InitialDirectory))
            {
                Directory.CreateDirectory(SaveDialog.InitialDirectory);
            }
            if (SaveDialog.ShowDialog() == DialogResult.OK)
            {
                XmlHelper.WriteXmlSet(SaveDialog.FileName);
            }
        }

        private void ReadSet_Click(object sender, EventArgs e)
        {
            if (FileDialog.ShowDialog() == DialogResult.OK)
            {
                XmlHelper.ReadXmlSet(FileDialog.FileName);
                promptOnExit = true;
            }
        }


        private void Apply_Click(object sender, EventArgs e)
        {
            BigTitle.Visible = false;
            SmallTitle.Text = "Applying";
            MainProgressBar.Maximum = 1000;
            MainProgressBar.Value = 0;
            MainProgressBar.Visible = true;

            Action action = new Action(Core.ApplyChanges);
            processing = true;
            action.BeginInvoke(UniversalAsyncCallback, null);

            while (processing)
            {
                SmallTitle.Text = "Applying " + (ApplyProgress.current + 1) + "/" + ApplyProgress.total;
                if (ApplyProgress.total != 0)
                {
                    MainProgressBar.Value = MainProgressBar.Maximum * ApplyProgress.current / ApplyProgress.total;
                }
                Application.DoEvents();
            }

            MainProgressBar.Value = MainProgressBar.Maximum;
            SmallTitle.Text = "ExMatics";
            MainProgressBar.Visible = false;
            MainProgressBar.Value = 0;
            BigTitle.Visible = true;
            promptOnExit = false;

            MessageBox.Show("Finished! ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion

        private void UniversalAsyncCallback(IAsyncResult ar)
        {
            processing = false;
        }

        private void CleanUpButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Clean up: This button would delete all unused files in the 'Resources' path. Are you sure to continue? ", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Core.CleanUp();
            }
        }

        private void ManageSaves_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(Shared.SaveDir))
            {
                Directory.CreateDirectory(Shared.SaveDir);
            }

            SavesManager manager = new SavesManager();
            manager.ShowDialog();
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
