using System;
using System.IO;

namespace LibNBT
{
    public class TagInt : AbstractTag
    {
        const int PAYLOAD_SIZE = 4;

        public int Value { get; set; }

        public override TagType Type
        {
            get { return TagType.Int; }
        }

        public override void Write(Stream output)
        {
            output.WriteByte((byte)Type);
            TagString.WriteString(output, Name);
            TagInt.WriteInt(output, Value);
        }

        internal static void WriteInt(Stream output, int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
            {
                BitHelper.SwapBytes(bytes, 0, PAYLOAD_SIZE);
            }

            output.Write(bytes, 0, PAYLOAD_SIZE);
        }

        public TagInt()
        {
            Name = string.Empty;
            Value = 0;
        }

        public TagInt(Stream input)
        {
            Name = TagString.ReadString(input);
            Value = ReadInt(input);
        }

        internal static int ReadInt(Stream input)
        {

            byte[] bytes = new byte[PAYLOAD_SIZE];
            if (PAYLOAD_SIZE != input.Read(bytes, 0, PAYLOAD_SIZE))
            {
                throw new Exception();
            }

            if (BitConverter.IsLittleEndian)
            {
                BitHelper.SwapBytes(bytes, 0, PAYLOAD_SIZE);
            }

            return BitConverter.ToInt32(bytes, 0);
        }

        internal static TagInt ReadUnnamedTagInt(Stream input)
        {
            return new TagInt() { Value = ReadInt(input) };
        }

        public override void WriteUnnamed(Stream output)
        {
            WriteInt(output, Value);
        }

        public override string ToString(string indentString)
        {
            return string.Format("{0}[Int: {1}={2}]", indentString, Name, Value);
        }
    }
}
