using LauncherCore;
using System;
using System.IO;
using System.Windows.Forms;

namespace LauncherUI
{
    public partial class MapRenameForm : Form
    {
        private Save currentSave;

        public MapRenameForm(ref Save save)
        {
            InitializeComponent();

            currentSave = save;
            FolderBox.Text = save.FolderName;
            LevelBox.Text = save.LevelName;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentSave.LevelName != LevelBox.Text)
                {
                    currentSave.LevelName = LevelBox.Text;
                }

                if (currentSave.FolderName != FolderBox.Text)
                {
                    currentSave.FolderName = FolderBox.Text;
                }
            }
            catch (PlatformNotSupportedException)
            {
                MessageBox.Show("Please notice that changing the folder name of a server map is invalid. ", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("An unexpected exception happened, maybe the map is broken. ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
