using System;
using System.IO;
using NBT.Exceptions;

namespace NBT.Tags
{
    public sealed class TagByte : Tag, IEquatable<TagByte>
    {
        public byte value;

        public TagByte() : this((byte)0)
        {
        }

        public TagByte(byte value)
        {
            this.value = value;
        }

        internal TagByte(Stream stream) : this((byte)0)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            this.readTag(stream);
        }

        public override byte tagID
        {
            get
            {
                return TagTypes.TagByte;            
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
            this.value = TagByte.ReadByte(stream);
        }

        internal override void writeTag(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            TagByte.WriteByte(stream, this.value);
        }

        internal static byte ReadByte(Stream stream)
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
            return (byte)num;
        }

        internal static void WriteByte(Stream stream, byte value)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            stream.WriteByte(value);
        }

        public override object Clone()
        {
            return new TagByte(this.value);
        }

        public static explicit operator TagByte(byte value)
        {
            return new TagByte(value);
        }

        public override Type getType()
        {
            return typeof(TagByte);
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
                if (value.GetType() != typeof(byte))
                {
                    throw new NBT_InvalidArgumentException();
                }
                this.value = (byte)value;
            }
        }

        public bool Equals(TagByte other)
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

            if (typeof(TagByte) != other.getType())
            {
                bResult = false;
            }
            else
            {
                bResult = this.Equals((TagByte)other);
            }

            return bResult;
        }
    }
}
