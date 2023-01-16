using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;

namespace LauncherCore
{
    public partial class Save
    {
        public override void ExportTo(string dest)
        {
            CurrentProgress.status = "Preparing";

            string tmpDir = SharedData.WorkingPath + "\\Tmp\\";
            Directory.CreateDirectory(tmpDir);
            Core.CopyDirectory(FolderPath, tmpDir + "save");
            Modset tmp = new Modset(ref SharedData.Mods);
            tmp.ExportTo(tmpDir + "Set.xml");
            tmp.Unload();

            if (Directory.Exists(SharedData.ScriptPath))
            {
                Core.CopyDirectory(SharedData.ScriptPath, tmpDir + "scripts");
            }

            if (Directory.Exists(SharedData.VoxelDataPath + FolderName))
            {
                Core.CopyDirectory(SharedData.VoxelDataPath + FolderName, tmpDir + "voxel");
            }

            string waypoints;
            if (File.Exists(waypoints = SharedData.GamePath + $"\\voxelmap\\{FolderName}.points"))
            {
                File.Copy(waypoints, tmpDir + "waypoints.points");
            }

            CurrentProgress.status = "Compressing";

            FastZip zip = new FastZip();
            zip.CreateZip(dest, tmpDir, true, null);

            CurrentProgress.status = "Cleaning up";

            Directory.Delete(tmpDir, true);
            CurrentProgress.Initialize();
        }

        public static void ImportFrom(string filePath)
        {
            CurrentProgress.status = "Extracting";

            string tmpDir = SharedData.WorkingPath + "\\Tmp\\";
            Directory.CreateDirectory(tmpDir);

            string zipName = filePath.Substring(filePath.LastIndexOf('\\') + 1);
            zipName = zipName.Substring(0, zipName.Length - 4);
            FastZip zip = new FastZip();
            zip.ExtractZip(filePath, tmpDir, null);

            Save save = new Save(tmpDir + "save");
            string destDir = SharedData.SavePath + save.LevelName;

            if (Directory.Exists(destDir))
            {
                if (File.Exists(destDir + "\\level.dat"))
                {
                    MessageResult result = SharedData.LogMessage("Destination directory already exists, do you wish to override it? Choose OK to override it, choose Cancel to cancel the current operation. ", "Prompt", MessageType.OKCancelQuestion);
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

            if (SharedData.LogMessage("Do you wish to override the current modset with the map's? ", "Question", MessageType.YesNoQuestion) == MessageResult.Yes)
            {
                Modset tmp = new Modset(tmpDir + "Set.xml");
                tmp.Apply();
                tmp.Unload();
            }

            if (Directory.Exists(tmpDir + "scripts"))
            {
                Core.CopyDirectory(tmpDir + "scripts", SharedData.ScriptPath);
            }

            if (Directory.Exists(tmpDir + "voxel"))
            {
                Core.CopyDirectory(tmpDir + "voxel", SharedData.VoxelDataPath + save.LevelName);
            }

            if (File.Exists(tmpDir + "waypoints.points"))
            {
                File.Copy(tmpDir + "waypoints.points", SharedData.GamePath + $"\\voxelmap\\{save.LevelName}.points");
            }

        CleanUp: CurrentProgress.status = "Cleaning up";

            Directory.Delete(tmpDir, true);
            CurrentProgress.Initialize();
        }
    }
}
