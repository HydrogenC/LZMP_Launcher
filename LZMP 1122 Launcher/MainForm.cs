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
        private Dictionary<String, Mod> mods = DefineMods.ReturnMods();
        public String resDir = Direct.GetCurrentDirectory() + "\\Resources\\";

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
            ReadModVersions();
            WriteInNodes();
            CheckIfModsExsist();
            SaveDialog.InitialDirectory = Direct.GetCurrentDirectory() + "\\Sets\\";
        }

        #region Initialize
        private void ReadModVersions()
        {
            try
            {
                String[] version = Files.ReadAllLines(Direct.GetCurrentDirectory() + "\\ModsVer.txt");
                BigTitle.Text += version[0].Substring(4);
                String crtKey = "";
                Int16 ctr = 0;
                for (Int16 i = 1; i < version.Length; i += 1)
                {
                    if (version[i] == "")
                    {
                        continue;
                    }
                    if (version[i].StartsWith("Key="))
                    {
                        crtKey = version[i].Substring(4);
                        ctr = 0;
                        continue;
                    }
                    mods[crtKey].Files[ctr] = mods[crtKey].Files[ctr].Replace("%v", version[i]);
                    if (!Files.Exists(resDir + mods[crtKey].Files[ctr] + ".jar"))
                    {
                        MessageBox.Show("File not found: " + mods[crtKey].Name + "\r\n" + mods[crtKey].Files[ctr], "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    ctr += 1;
                }
            }
            catch (System.IO.FileNotFoundException e)
            {
                MessageBox.Show("Config file not found, mod names are not imported! ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception e)
            {
                MessageBox.Show("An internal error occured, mod names are not imported! ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WriteInNodes()
        {
            MainTree.Nodes.Clear();
            MainTree.Nodes.Add("Technology Mods");
            MainTree.Nodes.Add("Warfare Mods");
            MainTree.Nodes.Add("Enhancement Mods");
            MainTree.ExpandAll();
            foreach (var i in mods)
            {
                i.Value.AddNode(MainTree);
            }
        }

        private void CheckIfModsExsist()
        {
            foreach (var i in mods)
            {
                i.Value.Node.Checked = i.Value.CheckInstalled(GameType.Client) || i.Value.CheckInstalled(GameType.Server);
            }
        }
        #endregion

        private void ApplyChanges()
        {
            Processing processing = new Processing();
            processing.Show();
            Int16 crtIndex = 0;
            foreach (var i in mods)
            {
                if (i.Value.Node.Checked && (!i.Value.Installed) && i.Value.Available)
                {
                    foreach (var j in i.Value.Files)
                    {
                        try
                        {
                            Files.Copy(resDir + j + ".jar", GameType.Client.ModDirectory + j + ".jar");
                            Files.Copy(resDir + j + ".jar", GameType.Server.ModDirectory + j + ".jar");
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("An internal error occured while copying files. ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                if ((!i.Value.Node.Checked) && i.Value.Installed)
                {
                    foreach (var j in i.Value.Files)
                    {
                        try
                        {
                            Files.Delete(GameType.Client.ModDirectory + j + ".jar");
                            Files.Delete(GameType.Server.ModDirectory + j + ".jar");
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("An internal error occured while deleting files. ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                crtIndex += 1;
                processing.SetProgress((UInt16)Math.Floor(100.0 * (Double)crtIndex / (Double)mods.Count));
            }
            CheckIfModsExsist();
            processing.Close();
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
                for (int j = 0; j < MainTree.Nodes[i].GetNodeCount(false); j += 1)
                {
                    if (MainTree.Nodes[i].Nodes[j].Checked)
                    {
                        MainTree.Nodes[i].Nodes[j].Checked = false;
                    }
                }
            }
        }

        private void LaunchClient_Click(object sender, EventArgs e)
        {
            ApplyChanges();
            String tmp = Direct.GetCurrentDirectory();
            Direct.SetCurrentDirectory(Direct.GetCurrentDirectory() + "\\Client\\");
            System.Diagnostics.Process.Start(GameType.Client.LauncherDirectory);
            Direct.SetCurrentDirectory(tmp);
        }

        private void LaunchServer_Click(object sender, EventArgs e)
        {
            ApplyChanges();
            String tmp = Direct.GetCurrentDirectory();
            Direct.SetCurrentDirectory(Direct.GetCurrentDirectory() + "\\Server\\");
            System.Diagnostics.Process.Start(GameType.Server.LauncherDirectory);
            Direct.SetCurrentDirectory(tmp);
        }

        private void SaveSet_Click(object sender, EventArgs e)
        {
            ApplyChanges();
            if (!Direct.Exists(SaveDialog.InitialDirectory))
            {
                Direct.CreateDirectory(SaveDialog.InitialDirectory);
            }
            if (SaveDialog.ShowDialog() == DialogResult.OK)
            {
                using (System.IO.FileStream fileStream = new System.IO.FileStream(SaveDialog.FileName, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Read))
                {
                    fileStream.Lock(0, fileStream.Length);
                    System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(fileStream);
                    foreach (var i in mods)
                    {
                        streamWriter.WriteLine(i.Key + "=" + i.Value.Installed);
                    }
                    fileStream.Unlock(0, fileStream.Length);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
            }
        }

        private void ReadSet_Click(object sender, EventArgs e)
        {
            if (FileDialog.ShowDialog() == DialogResult.OK)
            {
                String[] temp = Files.ReadAllLines(FileDialog.FileName);
                foreach (var i in temp)
                {
                    Int32 index = i.IndexOf('=');
                    String id = i.Substring(0, index);
                    Boolean check = Boolean.Parse(i.Substring(index + 1));
                    if (check && (!mods[id].Node.Checked))
                    {
                        mods[id].Node.Checked = true;
                    }
                    if ((!check) && mods[id].Node.Checked)
                    {
                        mods[id].Node.Checked = false;
                    }
                }
            }
        }

        private void Apply_Click(object sender, EventArgs e)
        {
            ApplyChanges();
            MessageBox.Show("Applied", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion
    }
}
