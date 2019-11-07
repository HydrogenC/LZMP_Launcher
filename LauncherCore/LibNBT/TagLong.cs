using System;
using System.IO;

namespace LibNBT
{
    public class TagLong : AbstractTag
    {
        const int PAYLOAD_SIZE = 8;

        public long Value { get; set; }

        public override TagType Type
        {
            get { return TagType.Long; }
        }

        public override void Write(Stream output)
        {
            output.WriteByte((byte)Type);
            TagString.WriteString(output, Name);
            TagLong.WriteLong(output, Value);
        }

        internal static void WriteLong(Stream output, long value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
            {
                BitHelper.SwapBytes(bytes, 0, PAYLOAD_SIZE);
            }

            output.Write(bytes, 0, PAYLOAD_SIZE);
        }

        public TagLong()
        {
            Name = string.Empty;
            Value = 0L;
        }

        public TagLong(Stream input)
        {
            Name = TagString.ReadString(input);
            Value = ReadLong(input);
        }

        internal static long ReadLong(Stream input)
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

            return BitConverter.ToInt64(bytes, 0);
        }

        internal static TagLong ReadUnnamedTagLong(Stream input)
        {
            return new TagLong() { Value = ReadLong(input) };
        }

        public override void WriteUnnamed(Stream output)
        {
            WriteLong(output, Value);
        }

        public override string ToString(string indentString)
        {
            return string.Format("{0}[Long: {1}={2}]", indentString, Name, Value);
        }
    }
}
