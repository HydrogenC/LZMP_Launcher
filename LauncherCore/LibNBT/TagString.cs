using System;
using System.IO;
using System.Text;

namespace LibNBT
{
    public class TagString : AbstractTag
    {
        public override TagType Type
        {
            get { return TagType.String; }
        }

        public string Value { get; set; }

        public TagString()
        {
            Name = string.Empty;
            Value = string.Empty;
        }

        public TagString(Stream input)
        {
            Name = ReadString(input);
            Value = ReadString(input);
        }

        public static string ReadString(Stream input)
        {
            short length = TagShort.ReadShort(input);
            byte[] bytes = new byte[length];
            if (length != input.Read(bytes, 0, length))
            {
                throw new Exception();
            }

            return Encoding.UTF8.GetString(bytes);
        }

        internal static TagString ReadUnnamedTagString(Stream input)
        {
            return new TagString() { Value = ReadString(input) };
        }

        internal static void WriteString(Stream output, string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            TagShort.WriteShort(output, (short)bytes.Length);

            output.Write(bytes, 0, bytes.Length);
        }

        public override void Write(Stream output)
        {
            output.WriteByte((byte)Type);
            TagString.WriteString(output, Name);
            TagString.WriteString(output, this.Value);
        }

        public override void WriteUnnamed(Stream output)
        {
            WriteString(output, Value);
        }

        public override string ToString(string indentString)
        {
            return string.Format("{0}[String: {1}=\"{2}\"]", indentString, Name, Value);
        }
    }
}
