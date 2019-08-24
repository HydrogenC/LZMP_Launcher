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

        public static void ExportSave(Save save, String zipFile, ref String status)
        {
            status = "Preparing";

            String tmpDir = Shared.WorkingDir + "\\Tmp\\";
            Directory.CreateDirectory(tmpDir);
            LauncherCore.CopyDirectory(save.Dir, tmpDir + "save\\");
            XmlHelper.WriteXmlSet(tmpDir + "Set.xml", false);

            if (Directory.Exists(Shared.ScriptDir))
            {
                LauncherCore.CopyDirectory(Shared.ScriptDir, tmpDir + "scripts\\");
            }

            if (Directory.Exists(Shared.JMDataDir + save.LevelName))
            {
                LauncherCore.CopyDirectory(Shared.JMDataDir + save.LevelName, tmpDir + "jm\\");
            }

            status = "Compressing";

            FastZip zip = new FastZip();
            zip.CreateZip(zipFile, tmpDir, true, null);

            status = "Cleaning up";

            Directory.Delete(tmpDir, true);
        }

        public static void ImportSave(String zipFile, ref String status)
        {
            OpenFileDialog xmlDialog = new OpenFileDialog();
            xmlDialog.Filter = "Xml File（*.xml）|*.xml";
            status = "Extracting";

            String tmpDir = Shared.WorkingDir + "\\Tmp\\";
            Directory.CreateDirectory(tmpDir);

            String zipName = zipFile.Substring(zipFile.LastIndexOf('\\') + 1);
            zipName = zipName.Substring(0, zipName.Length - 4);
            FastZip zip = new FastZip();
            zip.ExtractZip(zipFile, tmpDir, null);

            status = "Importing Map";

            Save save = new Save(tmpDir + "save");
            LauncherCore.CopyDirectory(tmpDir + "save", Shared.SaveDir + zipName);

            if (MessageBox.Show("Override the current modset with the map's? If you choose No, you can select where to save the map's modset later. ", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                status = "Importing modset";

                Int32 c = 0, t = 0;
                XmlHelper.ReadXmlSet(tmpDir + "Set.xml", false);
                LauncherCore.ApplyChanges(ref t, ref c);
                LauncherCore.CheckInstallation();
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
                LauncherCore.CopyDirectory(tmpDir + "scripts\\", Shared.ScriptDir);
            }

            if (Directory.Exists(tmpDir + "jm\\"))
            {
                LauncherCore.CopyDirectory(tmpDir + "jm\\", Shared.JMDataDir + save.LevelName);
            }

            status = "Cleaning up";

            Directory.Delete(tmpDir, true);
        }
    }
}
