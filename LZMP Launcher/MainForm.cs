using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using Direct = System.IO.Directory;
using Files = System.IO.File;

namespace LZMP_Launcher
{
    public partial class MainForm : Form
    {
        private Boolean processing = false;
        private Dictionary<String, Mod> mods = new Dictionary<String, Mod>();

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
            MainProgressBar.Visible = false;
            BigTitle.Visible = true;

            //XmlHelper.ReadDefinitions(GlobalResources.workingDir + "\\BasicSettings.xml", ref mods);
            XmlHelper.ReadDefinitions(@"C:\Users\Ailian Du\source\repos\LZMP_Launcher\LZMP Launcher\BasicSettings.xml", ref mods);
            BigTitle.Text += GlobalResources.version;

            WriteInNodes();

            foreach (var i in mods)
            {
                i.Value.AddNode(MainTree.Nodes[i.Value.Category].Nodes);
                i.Value.CheckAvailability();
            }

            CheckExistence();
            SaveDialog.InitialDirectory = GlobalResources.workingDir + "\\Sets\\";
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

        private void CheckExistence()
        {
            foreach (var i in mods)
            {
                i.Value.CheckInstalled();
                i.Value.Node.Checked = i.Value.Installed;
            }
        }
        #endregion

        private void ApplyChanges(Mod[] applyList)
        {
            Int32 crtIndex = 0;
            MainProgressBar.Step = (Int32)Math.Round((Double)MainProgressBar.Maximum / (Double)applyList.Length);
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
            CheckExistence();
        }

        private void ApplyCallback(IAsyncResult asyncResult)
        {
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

        private void LaunchClient_Click(object sender, EventArgs e)
        {
            Apply_Click(null, null);
            Direct.SetCurrentDirectory(GlobalResources.workingDir + "\\Client\\");
            System.Diagnostics.Process.Start(GlobalResources.clientLauncher);
            Direct.SetCurrentDirectory(GlobalResources.workingDir);
        }

        private void LaunchServer_Click(object sender, EventArgs e)
        {
            Apply_Click(null, null);
            Direct.SetCurrentDirectory(GlobalResources.workingDir + "\\Server\\");
            System.Diagnostics.Process.Start(GlobalResources.serverLauncher);
            Direct.SetCurrentDirectory(GlobalResources.workingDir);
        }

        private void SaveSet_Click(object sender, EventArgs e)
        {
            Apply_Click(null, null);
            if (!Direct.Exists(SaveDialog.InitialDirectory))
            {
                Direct.CreateDirectory(SaveDialog.InitialDirectory);
            }
            if (SaveDialog.ShowDialog() == DialogResult.OK)
            {
                XmlHelper.WriteXmlSet(SaveDialog.FileName, ref mods);
            }
        }

        private void ReadSet_Click(object sender, EventArgs e)
        {
            if (FileDialog.ShowDialog() == DialogResult.OK)
            {
                XmlHelper.ReadXmlSet(FileDialog.FileName, ref mods);
            }
        }

        private void Apply_Click(object sender, EventArgs e)
        {
            BigTitle.Visible = false;
            SmallTitle.Text = "Applying";
            MainProgressBar.Value = 0;
            MainProgressBar.Visible = true;
            List<Mod> applyList = new List<Mod>();
            foreach (var i in mods)
            {
                if (i.Value.Node.Checked != i.Value.Installed && i.Value.Available)
                {
                    applyList.Add(i.Value);
                    foreach (var j in i.Value.Addons)
                    {
                        if (j.Value.Node.Checked != j.Value.Installed && j.Value.Available)
                        {
                            applyList.Add(j.Value);
                        }
                    }
                }
            }
            Action<Mod[]> action = new Action<Mod[]>(ApplyChanges);
            action.BeginInvoke(applyList.ToArray(), ApplyCallback, null);
            processing = true;
            while (processing)
            {
                Application.DoEvents();
            }
            SmallTitle.Text = "ExMatics";
            MainProgressBar.Visible = false;
            MainProgressBar.Value = 0;
            BigTitle.Visible = true;
        }
        #endregion
    }
}
