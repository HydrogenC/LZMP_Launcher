using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;

namespace LauncherCore
{
    public class SavesHelper
    {
        public static Save[] GetSaves(MinecraftInstance instance)
        {
            List<Save> saves = new List<Save>();
            if (instance == SharedData.Client)
            {
                if (!Directory.Exists(SharedData.SavePath))
                {
                    Directory.CreateDirectory(SharedData.SavePath);
                }

                foreach (string i in Directory.GetDirectories(SharedData.SavePath))
                {
                    if (File.Exists(i + "\\level.dat"))
                    {
                        try
                        {
                            saves.Add(new Save(i));
                        }
                        catch (Exception) { }
                    }
                }
            }
            else
            {
                if (File.Exists(instance.GamePath + "\\world\\level.dat"))
                {
                    try
                    {
                        saves.Add(new Save(instance.GamePath + "\\world\\"));
                    }
                    catch (Exception) { }
                }
            }
            return saves.ToArray();
        }

        public static void ExportSave(Save save, string zipFile)
        {
            CurrentProgress.status = "Preparing";

            string tmpDir = MinecraftInstance.WorkingPath + "\\Tmp\\";
            Directory.CreateDirectory(tmpDir);
            Core.CopyDirectory(save.FolderPath, tmpDir + "save\\");
            XmlHelper.WriteXmlSet(tmpDir + "Set.xml", false);

            if (Directory.Exists(SharedData.Client.ScriptPath))
            {
                Core.CopyDirectory(SharedData.Client.ScriptPath, tmpDir + "scripts\\");
            }

            if (save.FolderPath.Substring(0, save.FolderPath.LastIndexOf('\\') + 1) == SharedData.SavePath && Directory.Exists(SharedData.JMDataPath + save.LevelName))
            {
                Core.CopyDirectory(SharedData.JMDataPath + save.LevelName, tmpDir + "jm\\");
            }

            CurrentProgress.status = "Compressing";

            FastZip zip = new FastZip();
            zip.CreateZip(zipFile, tmpDir, true, null);

            CurrentProgress.status = "Cleaning up";

            Directory.Delete(tmpDir, true);
            CurrentProgress.Initialize();
        }
        public static readonly Action<Save, string> ExportAction = ExportSave;

        public static void ImportSave(string zipFile, MinecraftInstance instance)
        {
            CurrentProgress.status = "Extracting";

            string tmpDir = MinecraftInstance.WorkingPath + "\\Tmp\\";
            Directory.CreateDirectory(tmpDir);

            string zipName = zipFile.Substring(zipFile.LastIndexOf('\\') + 1);
            zipName = zipName.Substring(0, zipName.Length - 4);
            FastZip zip = new FastZip();
            zip.ExtractZip(zipFile, tmpDir, null);

            Save save = new Save(tmpDir + "save");
            string destDir = (instance == SharedData.Client) ? SharedData.SavePath + save.LevelName : SharedData.Server.GamePath + "\\world";

            if (Directory.Exists(destDir))
            {
                if (File.Exists(destDir + "\\level.dat"))
                {
                    MessageResult result = SharedData.DisplayMessage("Destination directory already exists, do you wish to override it. Choose Yes to override it, choose No to export the map in the existing folder, choose Cancel to cancel the current operation. ", "Prompt", MessageType.YesNoCancelQuestion);
                    switch (result)
                    {
                        case MessageResult.Cancel:
                            goto CleanUp;
                        case MessageResult.Yes:
                            Directory.Delete(destDir, true);
                            break;
                        case MessageResult.No:
                            Save existingSave = new Save(destDir);
                            string exportPath = SharedData.SaveFile(existingSave.LevelName + ".zip", "Zip File（*.zip）|*.zip", null);
                            if (!string.IsNullOrEmpty(exportPath))
                            {
                                ExportSave(existingSave, exportPath);
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

            CurrentProgress.status = "Importing Map";
            Core.CopyDirectory(tmpDir + "save", destDir);

            if (SharedData.DisplayMessage("Override the current modset with the map's? If you choose No, you can select where to save the map's modset later. ", "Prompt", MessageType.YesNoQuestion) == MessageResult.Yes)
            {
                XmlHelper.ReadXmlSet(tmpDir + "Set.xml", false);
            }
            else
            {
                string fileName = SharedData.SaveFile(save.LevelName + ".xml", "Xml File（*.xml）|*.xml", null);
                if (!string.IsNullOrEmpty(fileName))
                {
                    File.Copy(tmpDir + "Set.xml", fileName, true);
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

        CleanUp: CurrentProgress.status = "Cleaning up";

            Directory.Delete(tmpDir, true);
            CurrentProgress.Initialize();
        }
        public static readonly Action<string, MinecraftInstance> ImportAction = ImportSave;
    }
}
