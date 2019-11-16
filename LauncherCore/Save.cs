using NbtLib;
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
            var nbt = NbtConvert.ParseNbtStream(File.OpenRead(Path + "\\level.dat"));

            levelName = ((NbtStringTag)(((NbtCompoundTag)nbt["Data"])["LevelName"])).ToString();
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
                var nbt = NbtConvert.ParseNbtStream(File.OpenRead(Path + "\\level.dat"));
                ((NbtCompoundTag)nbt["Data"])["LevelName"] = new NbtStringTag(value);
                var output = NbtConvert.CreateNbtStream(nbt);
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
