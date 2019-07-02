using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LZMP_Launcher
{
    public partial class MainForm : Form
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

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }
        #endregion

        public MainForm()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;
            MainProgressBar.Visible = false;
            BigTitle.Visible = true;

            XmlHelper.ReadDefinitions(Shared.workingDir + "\\BasicSettings.xml");
            BigTitle.Text += Shared.version;
            WriteInNodes();

            foreach (var i in Shared.mods)
            {
                switch (i.Value.Category)
                {
                    case ModCategory.Technology:
                        i.Value.AddNode(MainTree.Nodes[0].Nodes);
                        break;
                    case ModCategory.Warfare:
                        i.Value.AddNode(MainTree.Nodes[1].Nodes);
                        break;
                    case ModCategory.Enhancement:
                        i.Value.AddNode(MainTree.Nodes[2].Nodes);
                        break;
                    default:
                        break;
                }

                i.Value.CheckAvailability();
                foreach (var j in i.Value.Addons)
                {
                    j.Value.CheckAvailability();
                }
            }

            CheckInstallation();
            SaveDialog.InitialDirectory = Shared.workingDir + "\\Sets\\";
        }

        #region Initialize

        private void WriteInNodes()
        {
            MainTree.Nodes.Clear();
            MainTree.Nodes.Add("Technology Mods");
            MainTree.Nodes.Add("Warfare Mods");
            MainTree.Nodes.Add("Enhancement Mods");
            MainTree.ExpandAll();
        }

        private void CheckInstallation()
        {
            foreach (var i in Shared.mods)
            {
                i.Value.CheckInstalled();
                i.Value.Node.Checked = i.Value.Installed;

                foreach (var j in i.Value.Addons)
                {
                    j.Value.CheckInstalled();
                    j.Value.Node.Checked = j.Value.Installed;
                }
            }
        }
        #endregion

        private void ApplyChanges(Mod[] applyList)
        {
            Int32 crtIndex = 0;
            MainProgressBar.Value = 0;
            MainProgressBar.Maximum = 1000;
            MainProgressBar.Step = MainProgressBar.Maximum / applyList.Length;

            foreach (var i in applyList)
            {
                SmallTitle.Text = "Applying " + (crtIndex + 1) + "/" + applyList.Length;
                if (i.Installed)
                {
                    i.Uninstall();
                }
                else
                {
                    i.Install();
                }
                MainProgressBar.PerformStep();
                crtIndex += 1;
            }

            MainProgressBar.Value = MainProgressBar.Maximum;
            CheckInstallation();
            processing = false;
        }

        private void MainTree_AfterCheck(object sender, TreeViewEventArgs e)
        {
            TreeNode i = e.Node;

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
        }

        #region Buttons
        private void ExitForm_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void CheckAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < MainTree.GetNodeCount(false); i += 1)
            {
                if (!MainTree.Nodes[i].Checked)
                {
                    MainTree.Nodes[i].Checked = true;
                }
                for (int j = 0; j < MainTree.Nodes[i].GetNodeCount(false); j += 1)
                {
                    if (!MainTree.Nodes[i].Nodes[j].Checked)
                    {
                        MainTree.Nodes[i].Nodes[j].Checked = true;
                    }
                    for (int k = 0; k < MainTree.Nodes[i].Nodes[j].GetNodeCount(false); k += 1)
                    {
                        if (!MainTree.Nodes[i].Nodes[j].Nodes[k].Checked)
                        {
                            MainTree.Nodes[i].Nodes[j].Nodes[k].Checked = true;
                        }
                    }
                }
            }
        }

        private void CancelAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < MainTree.GetNodeCount(false); i += 1)
            {
                if (MainTree.Nodes[i].Checked)
                {
                    MainTree.Nodes[i].Checked = false;
                }
            }
        }

        private void LaunchButton_Click(object sender, EventArgs e)
        {
            Apply_Click(null, null);
            Directory.SetCurrentDirectory(Shared.workingDir + "\\Game\\");
            System.Diagnostics.Process.Start(Shared.launcher);
            Directory.SetCurrentDirectory(Shared.workingDir);
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
            }
        }

        private void Apply_Click(object sender, EventArgs e)
        {
            List<Mod> applyList = new List<Mod>();
            foreach (var i in Shared.mods)
            {
                if ((i.Value.Node.Checked != i.Value.Installed) && i.Value.Available)
                {
                    applyList.Add(i.Value);
                }

                foreach (var j in i.Value.Addons)
                {
                    if ((j.Value.Node.Checked != j.Value.Installed) && j.Value.Available)
                    {
                        applyList.Add(j.Value);
                    }
                }
            }

            if (applyList.Count == 0)
            {
                return;
            }

            BigTitle.Visible = false;
            SmallTitle.Text = "Applying";
            MainProgressBar.Value = 0;
            MainProgressBar.Visible = true;

            Action<Mod[]> action = new Action<Mod[]>(ApplyChanges);
            action.BeginInvoke(applyList.ToArray(), null, null);
            processing = true;
            while (processing)
            {
                Application.DoEvents();
            }

            SmallTitle.Text = "ExMatics";
            MainProgressBar.Visible = false;
            MainProgressBar.Value = 0;
            BigTitle.Visible = true;

            MessageBox.Show("Finished! ", "Information", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
        }
        #endregion

        private void CleanUpButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Clean up: This button would delete all unused files in the 'Resources' path. Are you sure to continue? ", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Cleaner.CleanUp();
            }
        }
    }
}
