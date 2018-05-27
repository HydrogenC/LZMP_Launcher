using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace WOT_Launcher
{
    public partial class MainForm : Form
    {
        private List<Mod> mods = DefineMods.ReturnMods();
        public String ResourceDir = System.IO.Directory.GetCurrentDirectory() + "\\Resources\\";

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
            WriteInNodes();
            CheckIfModsExsist();
            SaveDialog.InitialDirectory = System.IO.Directory.GetCurrentDirectory() + "\\Sets\\";
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
                i.AddNode(MainTree);
            }
        }

        private void CheckIfModsExsist()
        {
            foreach (var i in mods)
            {
                i.Node.Checked = i.CheckInstalled(GameType.Client) || i.CheckInstalled(GameType.Server);
            }
        }

        private void ApplyChanges()
        {
            foreach (var i in mods)
            {
                if (i.Node.Checked && (!i.Installed) && i.Available)
                {
                    foreach (var j in i.Files)
                    {
                        try
                        {
                            System.IO.File.Copy(ResourceDir + j + ".jar", GameType.Client.ModDirectory + j + ".jar");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Exception Caught: " + e);
                        }
                        try
                        {
                            System.IO.File.Copy(ResourceDir + j + ".jar", GameType.Server.ModDirectory + j + ".jar");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Exception Caught: " + e);
                        }
                    }
                }
                if ((!i.Node.Checked) && i.Installed)
                {
                    foreach (var j in i.Files)
                    {
                        try
                        {
                            System.IO.File.Delete(GameType.Client.ModDirectory + j + ".jar");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Exception Caught: " + e);
                        }
                        try
                        {
                            System.IO.File.Delete(GameType.Server.ModDirectory + j + ".jar");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Exception Caught: " + e);
                        }

                    }
                }
            }
            if (!mods[7].Node.Checked)
            {
                try
                {
                    System.IO.File.Delete(GameType.Client.ModDirectory + "ImmersiveEngineering-0.12-82-core.jar");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception Caught: " + e);
                }
                try
                {
                    System.IO.File.Delete(GameType.Server.ModDirectory + "ImmersiveEngineering-0.12-82-core.jar");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception Caught: " + e);
                }
            }
            CheckIfModsExsist();
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
            String tmp = System.IO.Directory.GetCurrentDirectory();
            System.IO.Directory.SetCurrentDirectory(System.IO.Directory.GetCurrentDirectory() + "\\Client");
            System.Diagnostics.Process.Start(GameType.Client.Launcher);
            System.IO.Directory.SetCurrentDirectory(tmp);
        }

        private void LaunchServer_Click(object sender, EventArgs e)
        {
            ApplyChanges();
            System.Diagnostics.Process.Start(GameType.Server.Launcher);
        }

        private void SaveSet_Click(object sender, EventArgs e)
        {
            ApplyChanges();
            if (!System.IO.Directory.Exists(SaveDialog.InitialDirectory))
            {
                System.IO.Directory.CreateDirectory(SaveDialog.InitialDirectory);
            }
            if (SaveDialog.ShowDialog() == DialogResult.OK)
            {
                using (System.IO.FileStream fileStream = new System.IO.FileStream(SaveDialog.FileName, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Read))
                {
                    fileStream.Lock(0, fileStream.Length);
                    System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(fileStream);
                    foreach (var i in mods)
                    {
                        streamWriter.WriteLine(i.Installed);
                    }
                    fileStream.Unlock(0, fileStream.Length);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                MessageBox.Show("Successfully saved! ", "Result");
            }
        }

        private void ReadSet_Click(object sender, EventArgs e)
        {
            if (FileDialog.ShowDialog() == DialogResult.OK)
            {
                String[] temp = System.IO.File.ReadAllLines(FileDialog.FileName);
                for (Int32 i = 0; i < mods.Count; i += 1)
                {
                    if (Boolean.Parse(temp[i]))
                    {
                        if (!mods[i].Node.Checked)
                        {
                            mods[i].Node.Checked = true;
                        }
                    }
                    else
                    {
                        if (mods[i].Node.Checked)
                        {
                            mods[i].Node.Checked = false;
                        }
                    }
                }
                MessageBox.Show("Read successfully! ", "Result");
            }
        }

        private void Apply_Click(object sender, EventArgs e)
        {
            ApplyChanges();
            MessageBox.Show("Applied successfully! ", "Result");
        }
        #endregion
    }
}
