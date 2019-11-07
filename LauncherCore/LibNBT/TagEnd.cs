using System.IO;

namespace LibNBT
{
    public class TagEnd : AbstractTag
    {
        public override TagType Type
        {
            get { return TagType.End; }
        }

        public override void Write(Stream output)
        {
            output.WriteByte((byte)Type);
        }

        public override string Name { get { return string.Empty; } set { } }

        public TagEnd() { }

        public static TagEnd Singleton = new TagEnd();

        public override void WriteUnnamed(Stream output) { }

        public override string ToString(string indentString)
        {
            return string.Format("{0}[End]", indentString);
        }
    }
}
