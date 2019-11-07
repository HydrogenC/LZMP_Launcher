using System;
using System.IO;
using System.Text;

namespace LibNBT
{
    public class TagIntArray : AbstractTag
    {
        public int[] Value { get; set; }

        public override TagType Type
        {
            get { return TagType.IntArray; }
        }

        public override void Write(System.IO.Stream output)
        {
            output.WriteByte((byte)Type);
            TagString.WriteString(output, Name);
            TagIntArray.WriteIntArray(output, Value);
        }

        private static void WriteIntArray(Stream output, int[] value)
        {
            if (value != null)
            {
                TagInt.WriteInt(output, value.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    TagInt.WriteInt(output, value[i]);
                }
            }
            else
            {
                TagInt.WriteInt(output, 0);
            }
        }

        public TagIntArray()
        {
            Name = string.Empty;
            Value = null;
        }

        public TagIntArray(Stream input)
        {
            Name = TagString.ReadString(input);
            Value = ReadIntArray(input);
        }

        internal static int[] ReadIntArray(Stream input)
        {
            int length = TagInt.ReadInt(input);
            int bufferLength = length * 4;

            byte[] buffer = new byte[bufferLength];
            if (bufferLength != input.Read(buffer, 0, bufferLength))
            {
                throw new Exception();
            }

            int[] ints = new int[length];
            for (int i = 0; i < length; i++)
            {
                if (BitConverter.IsLittleEndian)
                {
                    BitHelper.SwapBytes(buffer, i * 4, 4);
                }

                ints[i] = BitConverter.ToInt32(buffer, i * 4);
            }

            buffer = null;

            return ints;
        }


        public override void WriteUnnamed(System.IO.Stream output)
        {
            WriteIntArray(output, Value);
        }

        public override string ToString(string indentString)
        {
            StringBuilder sb = new StringBuilder();

            if (Value == null || Value.Length == 0)
            {
                return string.Format("{0}[Int_Array: {1}]", indentString, Name);
            }

            sb.AppendLine(string.Format("{0}[Int_Array: {1}", indentString, Name));

            foreach (int item in Value)
            {
                sb.AppendLine(string.Format("{0}  {1}", indentString, item));
            }

            sb.AppendLine(string.Format("{0}]", indentString));

            return sb.ToString();
        }

        internal static AbstractTag ReadUnnamedTagIntArray(Stream input)
        {
            return new TagIntArray() { Value = ReadIntArray(input) };
        }
    }
}
