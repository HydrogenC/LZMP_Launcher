using LibNBT;
using System;
using System.IO;

namespace LauncherCore
{
    public class Save
    {
        private string levelName, folderName;

        public Save(string dir)
        {
            if (dir.EndsWith("\\"))
            {
                dir = dir.Substring(0, dir.Length - 1);
            }

            folderName = dir.Substring(dir.LastIndexOf('\\') + 1);
            Path = dir;
            TagCompound tag = AbstractTag.ReadFromFile(Path + "\\level.dat") as TagCompound;
            levelName = tag.GetCompound("Data").GetString("LevelName").Value;
        }

        public void Delete()
        {
            Directory.Delete(Path, true);
        }

        public string Path
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
                    throw new PlatformNotSupportedException();
                }
                else
                {
                    Directory.Move(Path, SharedData.SavePath + value);
                    folderName = value;
                }
            }
        }

        public string LevelName
        {
            get => levelName;
            set
            {
                TagCompound tag = AbstractTag.ReadFromFile(Path + "\\level.dat") as TagCompound;
                TagString lvName = new TagString();
                lvName.Value = value;
                tag.GetCompound("Data").SetTag("LevelName", lvName);
                tag.WriteToFile(Path + "\\level.dat");
                levelName = value;
            }
        }

        public override string ToString()
        {
            return " " + levelName + " (" + folderName + ")";
        }

        public string DisplayName
        {
            get => ToString();
        }
    }
}
