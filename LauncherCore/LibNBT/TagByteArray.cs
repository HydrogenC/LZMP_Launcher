using System;
using System.IO;

namespace LibNBT
{
    public class TagByteArray : AbstractTag
    {
        public byte[] Value { get; set; }

        public override TagType Type
        {
            get { return TagType.ByteArray; }
        }

        public override void Write(Stream output)
        {
            output.WriteByte((byte)Type);
            TagString.WriteString(output, Name);
            TagByteArray.WriteByteArray(output, Value);
        }

        internal static void WriteByteArray(Stream output, byte[] value)
        {
            if (value != null)
            {
                TagInt.WriteInt(output, value.Length);
                output.Write(value, 0, value.Length);
            }
            else
            {
                TagInt.WriteInt(output, 0);
            }
        }

        public TagByteArray()
        {
            Name = string.Empty;
            Value = null;
        }

        public TagByteArray(Stream input)
        {
            Name = TagString.ReadString(input);
            Value = ReadByteArray(input);
        }

        internal static byte[] ReadByteArray(Stream input)
        {
            int length = TagInt.ReadInt(input);

            byte[] bytes = new byte[length];
            if (length != input.Read(bytes, 0, length))
            {
                throw new Exception();
            }

            return bytes;
        }

        internal static TagByteArray ReadUnnamedTagByteArray(Stream input)
        {
            return new TagByteArray() { Value = ReadByteArray(input) };
        }

        public override void WriteUnnamed(Stream output)
        {
            WriteByteArray(output, Value);
        }

        public override string ToString(string indentString)
        {
            return string.Format("{0}[Byte_Array: {1}={2} Bytes", indentString, Name, (Value != null) ? Value.Length : 0);
        }
    }
}
