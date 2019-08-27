using LibNBT;
using System;
using System.IO;

namespace LauncherCore
{
    public class Save
    {
        private String levelName, folderName;

        public Save(String dir)
        {
            if (dir.EndsWith("\\"))
            {
                dir = dir.Substring(0, dir.Length - 1);
            }

            folderName = dir.Substring(dir.LastIndexOf('\\') + 1);
            Dir = dir + "\\";
            TagCompound tag = AbstractTag.ReadFromFile(Dir + "level.dat") as TagCompound;
            levelName = tag.GetCompound("Data").GetString("LevelName").Value;
        }

        public String Dir
        {
            get;
            private set;
        }

        public String FolderName
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
                    Directory.Move(Dir, SharedData.SavePath + value);
                    folderName = value;
                }
            }
        }

        public String LevelName
        {
            get => levelName;
            set
            {
                TagCompound tag = AbstractTag.ReadFromFile(Dir + "level.dat") as TagCompound;
                TagString lvName = new TagString();
                lvName.Value = value;
                tag.GetCompound("Data").SetTag("LevelName", lvName);
                tag.WriteToFile(Dir + "level.dat");
                levelName = value;
            }
        }

        public override String ToString()
        {
            return " " + levelName + " (" + folderName + ")";
        }
    }
}
