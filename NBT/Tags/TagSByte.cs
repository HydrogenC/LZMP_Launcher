using System;
using System.IO;
using NBT.Exceptions;

namespace NBT.Tags
{
    public sealed class TagSByte : Tag, IEquatable<TagSByte>
    {
        public SByte value;

        public TagSByte() : this((sbyte)0)
        {
        }

        public TagSByte(sbyte value)
        {
            this.value = value;
        }

        internal TagSByte(Stream stream) : this((sbyte)0)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            this.readTag(stream);
        }

        public override object Value
        {
            get
            {
                return this.value;
            }
            set
            {
                if (value == null)
                {
                    throw new NBT_InvalidArgumentNullException();
                }
                if (value.GetType() != typeof(sbyte))
                {
                    throw new NBT_InvalidArgumentException();
                }
                this.value = (sbyte)value;
            }
        }

        public override byte tagID
        {
            get 
            {
                return TagTypes.TagSByte; 
            }
        }

        public override string toString()
        {
            return this.value.ToString();
        }

        internal override void readTag(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            this.value = TagSByte.ReadSByte(stream);
        }

        internal override void writeTag(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            TagSByte.WriteSByte(stream, this.value);
        }

        internal static sbyte ReadSByte(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            int num = stream.ReadByte();
            if (num == -1)
            {
                throw new NBT_EndOfStreamException();
            }
            return (sbyte)num;
        }

        internal static void WriteSByte(Stream stream, sbyte value)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            stream.WriteByte((byte)value);
        }

        public override object Clone()
        {
            return new TagSByte(this.value);
        }

        public static explicit operator TagSByte(sbyte value)
        {
            return new TagSByte(value);
        }

        public override Type getType()
        {
            return typeof(TagSByte);
        }

        public bool Equals(TagSByte other)
        {
            bool bResult = false;
            try
            {
                bResult = this.value.Equals(other.value);
            }
            catch (ArgumentNullException nullEx)
            {
                throw new NBT_InvalidArgumentNullException(nullEx.Message, nullEx.InnerException);
            }
            catch (Exception ex)
            {
                throw new NBT_InvalidArgumentException(ex.Message, ex.InnerException);
            }
            return bResult;
        }

        public override bool Equals(Tag other)
        {
            bool bResult = true;

            if (typeof(TagSByte) != other.getType())
            {
                bResult = false;
            }
            else
            {
                bResult = this.Equals((TagSByte)other);
            }

            return bResult;
        }
    }
}
