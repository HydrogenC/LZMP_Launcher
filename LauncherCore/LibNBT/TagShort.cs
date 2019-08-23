using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LibNBT
{
    public class TagShort : AbstractTag
    {
        const int PAYLOAD_SIZE = 2;

        public short Value { get; set; }
        
        public TagShort()
        {
            Name = String.Empty;
            Value = 0;
        }
        
        public TagShort(Stream input)
        {
            Name = TagString.ReadString(input);
            Value = ReadShort(input);
        }
        public override TagType Type
        {
            get { return TagType.Short; }
        }

        public override void Write(Stream output)
        {
            output.WriteByte((byte)Type);
            TagString.WriteString(output, Name);
            TagShort.WriteShort(output, Value);
        }

        internal static void WriteShort(Stream output, short value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
            {
                BitHelper.SwapBytes(bytes, 0, PAYLOAD_SIZE);
            }

            output.Write(bytes, 0, PAYLOAD_SIZE);
        }

        internal static short ReadShort(Stream input)
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

            return BitConverter.ToInt16(bytes, 0);
        }

        internal static TagShort ReadUnnamedTagShort(Stream input)
        {
            return new TagShort() { Value = ReadShort(input) };
        }

        public override void WriteUnnamed(Stream output)
        {
            WriteShort(output, Value);
        }

        public override string ToString(string indentString)
        {
            return String.Format("{0}[Short: {1}={2}]", indentString, Name, Value);
        }
    }
}
