using NBT.IO;
using NBT.Tags;
using System;
using System.IO;

namespace LauncherCore
{
    public class Save
    {
        private string folderName, levelName;
        private NBTFile nbtFile = new NBTFile();

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
        public void Delete()
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
                if (Directory.Exists(SharedData.SavePath + value))
                {
                    throw new IOException();
                }
                else
                {
                    Directory.Move(FolderPath, SharedData.SavePath + value);
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
                (nbtFile.RootTag["Data"] as TagCompound)["LevelName"].Value = value;
                nbtFile.Save(FolderPath + "\\level.dat");
                levelName = value;
            }
        }

        public override string ToString()
        {
            return " " + levelName + " (" + folderName + ")";
        }

        /// <summary>
        /// A property equivalant to <see cref="ToString"/> used in WPF. 
        /// </summary>
        public string DisplayName
        {
            get => ToString();
        }
    }
}
