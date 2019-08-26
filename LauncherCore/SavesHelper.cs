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
                foreach (String i in Directory.GetDirectories(instance.SaveDir))
                {
                    if (File.Exists(i + "\\level.dat"))
                    {
                        saves.Add(new Save(i));
                    }
                }
            }
            else
            {
                if (Directory.Exists(instance.GamePath + "\\world\\"))
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
            Core.CopyDirectory(save.Dir, tmpDir + "save\\");
            XmlHelper.WriteXmlSet(tmpDir + "Set.xml", false);

            if (Directory.Exists(save.Instance.ScriptDir))
            {
                Core.CopyDirectory(save.Instance.ScriptDir, tmpDir + "scripts\\");
            }

            if (Directory.Exists(save.Instance.JMDataDir + save.LevelName))
            {
                Core.CopyDirectory(save.Instance.JMDataDir + save.LevelName, tmpDir + "jm\\");
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

            SavesStatus.status = "Importing Map";

            Save save = new Save(tmpDir + "save");
            if (instance == SharedData.Client)
            {
                Core.CopyDirectory(tmpDir + "save", instance.SaveDir + zipName);
            }
            else
            {
                if (!Directory.Exists(SharedData.Server.GamePath + "\\world"))
                {
                    Core.CopyDirectory(tmpDir + "save", SharedData.Server.GamePath + "\\world");
                }
                else
                {
                    if (MessageBox.Show("Override the current server map? ", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Directory.Delete(SharedData.Server.GamePath + "\\world", true);
                        Core.CopyDirectory(tmpDir + "save", SharedData.Server.GamePath + "\\world");
                    }
                    else
                    {
                        return;
                    }
                }
            }

            if (MessageBox.Show("Override the current modset with the map's? If you choose No, you can select where to save the map's modset later. ", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                XmlHelper.ReadXmlSet(tmpDir + "Set.xml", false);
                Core.ApplyChanges(instance);
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
                Core.CopyDirectory(tmpDir + "scripts\\", instance.ScriptDir);
            }

            if (Directory.Exists(tmpDir + "jm\\"))
            {
                Core.CopyDirectory(tmpDir + "jm\\", instance.JMDataDir + save.LevelName);
            }

            SavesStatus.status = "Cleaning up";

            Directory.Delete(tmpDir, true);
            SavesStatus.Initialize();
        }
    }
}
