using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LibNBT
{
    public class TagByte: AbstractTag
    {
        public byte Value { get; set; }

        public override TagType Type
        {
            get { return TagType.Byte; }
        }

        public override void Write(Stream output)
        {
            output.WriteByte((byte)Type);
            TagString.WriteString(output, Name);
            TagByte.WriteByte(output, Value);
        }

        internal static void WriteByte(Stream output, byte value)
        {
            output.WriteByte(value);
        }

        public TagByte()
        {
            Name = String.Empty;
            Value = (byte)0;
        }

        public TagByte(Stream decompressedInput)
        {
            Name = TagString.ReadString(decompressedInput);
            Value = ReadByte(decompressedInput);
        }

        internal static byte ReadByte(Stream decompressedInput)
        {
            int temp = decompressedInput.ReadByte();
            if (temp == (temp & 0xFF))
            {
                return (byte)temp;
            }
            else
            {
                throw new Exception();
            }
        }


        internal static TagByte ReadUnnamedTagByte(Stream input)
        {
            return new TagByte() { Value = ReadByte(input) };
        }

        public override void WriteUnnamed(Stream output)
        {
            WriteByte(output, Value);
        }

        public override string ToString(string indentString)
        {
            return String.Format("{0}[Byte: {1}={2}]", indentString, Name, Value);
        }
    }
}
