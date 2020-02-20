using NBT.IO;
using NBT.Tags;
using System;
using System.Collections.Generic;
using System.IO;

namespace LauncherCore
{
    public partial class Save : IEditable
    {
        private string folderName, levelName;
        private NBTFile nbtFile = new NBTFile();

        public Save()
        {

        }

        public Save(string dir)
        {
            FolderPath = dir.EndsWith("\\") ? dir.Substring(0, dir.Length - 1) : dir;
            folderName = FolderPath.Substring(FolderPath.LastIndexOf('\\') + 1);
            nbtFile.Load(FolderPath + "\\level.dat");
            levelName = (nbtFile.RootTag["Data"] as TagCompound)["LevelName"].Value as string;
        }

        /// <summary>
        /// Delete the save. 
        /// </summary>
        public override void Delete()
        {
            Directory.Delete(FolderPath, true);
        }

        public string FolderPath
        {
            get;
            private set;
        }

        public string FolderName
        {
            get => folderName;
            set
            {
                if (folderName == value)
                {
                    return;
                }

                if (Directory.Exists(SharedData.SavePath + value))
                {
                    throw new IOException();
                }
                else
                {
                    Directory.Move(FolderPath, SharedData.SavePath + value);
                    if (Directory.Exists(SharedData.JMDataPath + folderName.Replace('-', '~')))
                    {
                        Directory.Move(SharedData.JMDataPath + folderName.Replace('-', '~'), SharedData.JMDataPath + value.Replace('-', '~'));
                    }
                    folderName = value;
                    FolderPath = FolderPath.Substring(0, FolderPath.LastIndexOf('\\') + 1) + value;
                }
            }
        }

        public string LevelName
        {
            get => levelName;
            set
            {
                if (levelName == value)
                {
                    return;
                }

                (nbtFile.RootTag["Data"] as TagCompound)["LevelName"].Value = value;
                nbtFile.Save(FolderPath + "\\level.dat");
                levelName = value;
            }
        }

        public override string ToString()
        {
            return " " + levelName + " (" + folderName + ")";
        }

        public override void Rename(string newName, bool type)
        {
            if (type)
            {
                FolderName = newName;
            }
            else
            {
                LevelName = newName;
            }
        }

        public override string IOFilter
        {
            get => "Zip File（*.zip）| *.zip";
        }

        public override string DisplayName
        {
            get => ToString();
        }
    }
}
