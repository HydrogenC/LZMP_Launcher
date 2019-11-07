using System;
using System.IO;

namespace LibNBT
{
    public class TagFloat : AbstractTag
    {
        const int PAYLOAD_SIZE = 4;

        public float Value { get; set; }

        public override TagType Type
        {
            get { return TagType.Float; }
        }

        public override void Write(Stream output)
        {
            output.WriteByte((byte)Type);
            TagString.WriteString(output, Name);
            TagFloat.WriteFloat(output, Value);
        }

        internal static void WriteFloat(Stream output, float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
            {
                BitHelper.SwapBytes(bytes, 0, PAYLOAD_SIZE);
            }

            output.Write(bytes, 0, PAYLOAD_SIZE);
        }

        public TagFloat()
        {
            Name = string.Empty;
            Value = 0;
        }

        public TagFloat(Stream input)
        {
            Name = TagString.ReadString(input);
            Value = ReadFloat(input);
        }

        internal static float ReadFloat(Stream input)
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

            return BitConverter.ToSingle(bytes, 0);
        }

        internal static TagFloat ReadUnnamedTagFloat(Stream input)
        {
            return new TagFloat() { Value = ReadFloat(input) };
        }

        public override void WriteUnnamed(Stream output)
        {
            WriteFloat(output, Value);
        }

        public override string ToString(string indentString)
        {
            return string.Format("{0}[Float: {1}={2}]", indentString, Name, Value);
        }
    }
}
