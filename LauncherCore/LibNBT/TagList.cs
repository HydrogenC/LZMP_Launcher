using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LibNBT
{
    public class TagList : AbstractTag
    {
        private List<AbstractTag> _value;
        public List<AbstractTag> Value
        {
            get { return _value; }
            set
            {
                if (value == null)
                {
                    _value = new List<AbstractTag>();
                }
                else
                {
                    _value = value;
                }
            }
        }

        public override TagType Type
        {
            get { return TagType.List; }
        }

        public override void Write(Stream output)
        {
            output.WriteByte((byte)Type);
            TagString.WriteString(output, Name);
            TagList.WriteList(output, Value);
        }

        internal static void WriteList(Stream output, List<AbstractTag> Value)
        {
            if (Value.Count > 0)
            {
                output.WriteByte((byte)Value[0].Type);
                TagInt.WriteInt(output, Value.Count);
                for (int i = 0; i < Value.Count; i++)
                {
                    Value[i].WriteUnnamed(output);
                }
            }
            else
            {
                output.WriteByte(0);
                output.WriteByte(0);
            }
        }

        public TagList()
        {
            Name = string.Empty;
            Value = new List<AbstractTag>();
        }

        public TagList(Stream input)
        {
            Name = TagString.ReadString(input);
            Value = ReadTagList(input);
        }

        internal static List<AbstractTag> ReadTagList(Stream input)
        {
            TagType tagType = (TagType)TagByte.ReadByte(input);
            int length = TagInt.ReadInt(input);

            List<AbstractTag> tags = new List<AbstractTag>();
            for (int i = 0; i < length; i++)
            {
                AbstractTag tag = null;

                switch (tagType)
                {
                    case TagType.Byte:
                        tag = TagByte.ReadUnnamedTagByte(input);
                        break;
                    case TagType.ByteArray:
                        tag = TagByteArray.ReadUnnamedTagByteArray(input);
                        break;
                    case TagType.Compound:
                        tag = TagCompound.ReadUnnamedTagCompound(input);
                        break;
                    case TagType.Double:
                        tag = TagDouble.ReadUnnamedTagDouble(input);
                        break;
                    case TagType.End:
                        tag = new TagEnd();
                        break;
                    case TagType.Float:
                        tag = TagFloat.ReadUnnamedTagFloat(input);
                        break;
                    case TagType.Int:
                        tag = TagInt.ReadUnnamedTagInt(input);
                        break;
                    case TagType.IntArray:
                        tag = TagIntArray.ReadUnnamedTagIntArray(input);
                        break;
                    case TagType.List:
                        tag = TagList.ReadUnnamedTagList(input);
                        break;
                    case TagType.Long:
                        tag = TagLong.ReadUnnamedTagLong(input);
                        break;
                    case TagType.Short:
                        tag = TagShort.ReadUnnamedTagShort(input);
                        break;
                    case TagType.String:
                        tag = TagString.ReadUnnamedTagString(input);
                        break;
                    default:
                        tag = null;
                        break;
                }

                if (tag != null)
                {
                    tags.Add(tag);
                }
            }

            return tags;
        }

        internal static AbstractTag ReadUnnamedTagList(Stream input)
        {
            return new TagList() { Value = ReadTagList(input) };
        }

        public override void WriteUnnamed(Stream output)
        {
            WriteList(output, Value);
        }

        public override string ToString(string indentString)
        {
            StringBuilder sb = new StringBuilder();

            if (Value.Count == 0)
            {
                return string.Format("{0}[List: {1}]", indentString, Name);
            }

            sb.AppendLine(string.Format("{0}[List: {1}", indentString, Name));

            foreach (AbstractTag item in _value)
            {
                sb.AppendLine(string.Format("{0}  {1}", indentString, item.ToString(indentString + "  ").Trim()));
            }

            sb.AppendLine(string.Format("{0}]", indentString));

            return sb.ToString();
        }
    }
}
