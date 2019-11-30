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
                    MessageResult result = SharedData.DisplayMessage("Destination directory already exists, do you wish to override it? Choose OK to override it, choose Cancel to cancel the current operation. ", "Prompt", MessageType.OKCancelQuestion);
                    switch (result)
                    {
                        case MessageResult.Cancel:
                            goto CleanUp;
                        case MessageResult.OK:
                            Directory.Delete(destDir, true);
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

            if (SharedData.DisplayMessage("Do you wish to override the current modset with the map's? ", "Question", MessageType.YesNoQuestion) == MessageResult.Yes)
            {
                XmlHelper.ReadXmlSet(tmpDir + "Set.xml", false);
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
