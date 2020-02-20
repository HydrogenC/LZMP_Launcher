namespace LauncherCore
{
    public class IEditable
    {
        public virtual string IOFilter
        {
            get;
        }

        public virtual void Delete() { }
        public virtual void ExportTo(string dest, bool showInfo) { }

        /// <summary>
        /// Rename the file
        /// </summary>
        /// <param name="newName">The new name for it</param>
        /// <param name="type">If true, rename the external name. Otherwise rename the internal name. </param>
        public virtual void Rename(string newName, bool type) { }

        public virtual string DisplayName
        {
            get;
        }
    }
}
