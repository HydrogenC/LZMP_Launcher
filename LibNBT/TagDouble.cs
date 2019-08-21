using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LibNBT
{
    public class TagDouble : AbstractTag
    {
        const int PAYLOAD_SIZE = 8;

        public double Value { get; set; }

        public override TagType Type
        {
            get { return TagType.Double; }
        }

        public override void Write(Stream output)
        {
            output.WriteByte((byte)Type);
            TagString.WriteString(output, Name);
            TagDouble.WriteDouble(output, Value);
        }

        internal static void WriteDouble(Stream output, double value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
            {
                BitHelper.SwapBytes(bytes, 0, PAYLOAD_SIZE);
            }

            output.Write(bytes, 0, PAYLOAD_SIZE);
        }

        public TagDouble()
        {
            Name = String.Empty;
            Value = 0;
        }

        public TagDouble(Stream input)
        {
            Name = TagString.ReadString(input);
            Value = ReadDouble(input);
        }

        internal static double ReadDouble(Stream input)
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

            return BitConverter.ToDouble(bytes, 0);
        }

        internal static TagDouble ReadUnnamedTagDouble(Stream input)
        {
            return new TagDouble() { Value = ReadDouble(input) };
        }

        public override void WriteUnnamed(Stream output)
        {
            WriteDouble(output, Value);
        }

        public override string ToString(string indentString)
        {
            return String.Format("{0}[Double: {1}={2}]", indentString, Name, Value);
        }
    }
}
