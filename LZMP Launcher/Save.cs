using LibNBT;
using System;

namespace LZMP_Launcher
{
    public class Save
    {
        public String FolderName
        {
            private set;
            get;
        }

        public String LevelName
        {
            private set;
            get;
        }

        public String Dir
        {
            private set;
            get;
        }

        public Save(String dir)
        {
            if (dir.EndsWith("\\"))
            {
                dir = dir.Substring(0, dir.Length - 1);
            }
            FolderName = dir.Substring(dir.LastIndexOf('\\') + 1);
            Dir = dir + "\\";
            TagCompound tag = AbstractTag.ReadFromFile(Dir + "level.dat") as TagCompound;
            LevelName = tag.GetCompound("Data").GetString("LevelName").Value;
        }

        public override String ToString()
        {
            return LevelName + " (" + FolderName + ")";
        }
    }
}
