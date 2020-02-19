namespace LauncherCore
{
    abstract class IEditableFile
    {
        public abstract void Delete();
        public static void ImportFrom(string source) { }
        public abstract void ExportTo(string dest);
        public abstract void Rename(string newName);
    }
}
