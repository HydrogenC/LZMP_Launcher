namespace LauncherCore
{
    public class EditableObject
    {
        public virtual string IOFilter
        {
            get;
        }

        public virtual void Delete() { }
        public virtual void ExportTo(string dest) { }

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
