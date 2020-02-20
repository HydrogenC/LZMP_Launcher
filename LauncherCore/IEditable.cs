namespace LauncherCore
{
    public abstract class IEditable
    {
        public abstract void Delete();
        public static void ImportFrom(string source) { }
        public abstract void ExportTo(string dest);

        /// <summary>
        /// Rename the file
        /// </summary>
        /// <param name="newName">The new name for it</param>
        /// <param name="type">If true, rename the external name. Otherwise rename the internal name. </param>
        public abstract void Rename(string newName,bool type);

        public abstract override string ToString();
    }
}
