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
        public static Save[] GetSaves(MinecraftInstance instance)
        {
            List<Save> saves = new List<Save>();
            if (instance == SharedData.Client)
            {
                foreach (String i in Directory.GetDirectories(SharedData.SavePath))
                {
                    if (File.Exists(i + "\\level.dat"))
                    {
                        saves.Add(new Save(i));
                    }
                }
            }
            else
            {
                if (File.Exists(instance.GamePath + "\\world\\level.dat"))
                {
                    saves.Add(new Save(instance.GamePath + "\\world\\"));
                }
            }
            return saves.ToArray();
        }

        public static void ExportSave(Save save, String zipFile)
        {
            SavesStatus.status = "Preparing";

            String tmpDir = MinecraftInstance.WorkingPath + "\\Tmp\\";
            Directory.CreateDirectory(tmpDir);
            Core.CopyDirectory(save.Path, tmpDir + "save\\");
            XmlHelper.WriteXmlSet(tmpDir + "Set.xml", false);

            if (Directory.Exists(SharedData.Client.ScriptPath))
            {
                Core.CopyDirectory(SharedData.Client.ScriptPath, tmpDir + "scripts\\");
            }

            if (save.Path.Substring(0, save.Path.LastIndexOf('\\') + 1) == SharedData.SavePath && Directory.Exists(SharedData.JMDataPath + save.LevelName))
            {
                Core.CopyDirectory(SharedData.JMDataPath + save.LevelName, tmpDir + "jm\\");
            }

            SavesStatus.status = "Compressing";

            FastZip zip = new FastZip();
            zip.CreateZip(zipFile, tmpDir, true, null);

            SavesStatus.status = "Cleaning up";

            Directory.Delete(tmpDir, true);
            SavesStatus.Initialize();
        }

        public static void ImportSave(String zipFile, MinecraftInstance instance)
        {
            OpenFileDialog xmlDialog = new OpenFileDialog();
            xmlDialog.Filter = "Xml File（*.xml）|*.xml";
            SavesStatus.status = "Extracting";

            String tmpDir = MinecraftInstance.WorkingPath + "\\Tmp\\";
            Directory.CreateDirectory(tmpDir);

            String zipName = zipFile.Substring(zipFile.LastIndexOf('\\') + 1);
            zipName = zipName.Substring(0, zipName.Length - 4);
            FastZip zip = new FastZip();
            zip.ExtractZip(zipFile, tmpDir, null);

            Save save = new Save(tmpDir + "save");
            String destDir = (instance == SharedData.Client) ? SharedData.SavePath + save.LevelName : SharedData.Server.GamePath + "\\world";

            if (Directory.Exists(destDir))
            {
                if (File.Exists(destDir + "\\level.dat"))
                {
                    DialogResult result = MessageBox.Show("Destination directory already exists, do you wish to override it. Choose Yes to override it, choose No to export the map in the existing folder, choose Cancel to cancel the current operation. ", "Prompt", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    switch (result)
                    {
                        case DialogResult.Cancel:
                            goto CleanUp;
                            break;
                        case DialogResult.Yes:
                            Directory.Delete(destDir, true);
                            break;
                        case DialogResult.No:
                            Save existingSave = new Save(destDir);
                            SaveFileDialog exportDialog = new SaveFileDialog();
                            exportDialog.Filter = "Zip File（*.zip）|*.zip";
                            exportDialog.FileName = existingSave.LevelName + ".zip";
                            if (exportDialog.ShowDialog() == DialogResult.OK)
                            {
                                ExportSave(existingSave, exportDialog.FileName);
                                Directory.Delete(destDir, true);
                            }
                            else
                            {
                                goto CleanUp;
                            }
                            break;
                    }
                }
                else
                {
                    Directory.Delete(destDir, true);
                }
            }

            SavesStatus.status = "Importing Map";
            Core.CopyDirectory(tmpDir + "save", destDir);

            if (MessageBox.Show("Override the current modset with the map's? If you choose No, you can select where to save the map's modset later. ", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                XmlHelper.ReadXmlSet(tmpDir + "Set.xml", false);
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
                Core.CopyDirectory(tmpDir + "scripts\\", SharedData.Client.ScriptPath);
            }

            if (instance == SharedData.Client && Directory.Exists(tmpDir + "jm\\"))
            {
                Core.CopyDirectory(tmpDir + "jm\\", SharedData.JMDataPath + save.LevelName);
            }

        CleanUp: SavesStatus.status = "Cleaning up";

            Directory.Delete(tmpDir, true);
            SavesStatus.Initialize();
        }
    }
}
