using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;
using System.Windows.Forms;

namespace LauncherCore
{
    public class SavesHelper
    {
        public static Save[] GetSaves()
        {
            List<Save> saves = new List<Save>();
            foreach (String i in Directory.GetDirectories(Shared.SaveDir))
            {
                if (File.Exists(i + "\\level.dat"))
                {
                    saves.Add(new Save(i));
                }
            }
            return saves.ToArray();
        }

        public static void ExportSave(Save save, String zipFile)
        {
            SavesStatus.status = "Preparing";

            String tmpDir = Shared.WorkingDir + "\\Tmp\\";
            Directory.CreateDirectory(tmpDir);
            Core.CopyDirectory(save.Dir, tmpDir + "save\\");
            XmlHelper.WriteXmlSet(tmpDir + "Set.xml", false);

            if (Directory.Exists(Shared.ScriptDir))
            {
                Core.CopyDirectory(Shared.ScriptDir, tmpDir + "scripts\\");
            }

            if (Directory.Exists(Shared.JMDataDir + save.LevelName))
            {
                Core.CopyDirectory(Shared.JMDataDir + save.LevelName, tmpDir + "jm\\");
            }

            SavesStatus.status = "Compressing";

            FastZip zip = new FastZip();
            zip.CreateZip(zipFile, tmpDir, true, null);

            SavesStatus.status = "Cleaning up";

            Directory.Delete(tmpDir, true);
            SavesStatus.Initialize();
        }

        public static void ImportSave(String zipFile)
        {
            OpenFileDialog xmlDialog = new OpenFileDialog();
            xmlDialog.Filter = "Xml File（*.xml）|*.xml";
            SavesStatus.status = "Extracting";

            String tmpDir = Shared.WorkingDir + "\\Tmp\\";
            Directory.CreateDirectory(tmpDir);

            String zipName = zipFile.Substring(zipFile.LastIndexOf('\\') + 1);
            zipName = zipName.Substring(0, zipName.Length - 4);
            FastZip zip = new FastZip();
            zip.ExtractZip(zipFile, tmpDir, null);

            SavesStatus.status = "Importing Map";

            Save save = new Save(tmpDir + "save");
            Core.CopyDirectory(tmpDir + "save", Shared.SaveDir + zipName);

            if (MessageBox.Show("Override the current modset with the map's? If you choose No, you can select where to save the map's modset later. ", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SavesStatus.status = "Importing modset";

                XmlHelper.ReadXmlSet(tmpDir + "Set.xml", false);
                Core.ApplyChanges();
                Core.CheckInstallation();
            }
            else
            {
                if (xmlDialog.ShowDialog() == DialogResult.OK)
                {
                    File.Copy(tmpDir + "Set.xml", xmlDialog.FileName, true);
                }
            }

            if (Directory.Exists(tmpDir + "scripts\\"))
            {
                Core.CopyDirectory(tmpDir + "scripts\\", Shared.ScriptDir);
            }

            if (Directory.Exists(tmpDir + "jm\\"))
            {
                Core.CopyDirectory(tmpDir + "jm\\", Shared.JMDataDir + save.LevelName);
            }

            SavesStatus.status = "Cleaning up";

            Directory.Delete(tmpDir, true);
            SavesStatus.Initialize();
        }
    }
}
