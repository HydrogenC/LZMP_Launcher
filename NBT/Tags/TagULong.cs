using System;
using System.IO;
using NBT.Exceptions;

namespace NBT.Tags
{
    public sealed class TagULong : Tag, IEquatable<TagULong>
    {
        public ulong value;

        public TagULong() : this((ulong)0)
        {
        }

        public TagULong(ulong value)
        {
            this.value = value;
        }

        internal TagULong(Stream stream) : this((ulong)0)
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
                if (value.GetType() != typeof(ulong))
                {
                    throw new NBT_InvalidArgumentException();
                }
                this.value = (ulong)value;
            }
        }

        public override byte tagID
        {
            get 
            {
                return TagTypes.TagULong; 
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
            this.value = TagULong.ReadULong(stream);
        }

        internal override void writeTag(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            TagULong.WriteULong(stream, this.value);
        }

        internal static ulong ReadULong(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            byte[] buffer = new byte[8];
            if (stream.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                throw new NBT_EndOfStreamException();
            }
            if (BitConverter.IsLittleEndian == true)
            {
                Array.Reverse(buffer);
            }
            return BitConverter.ToUInt64(buffer, 0);
        }

        internal static void WriteULong(Stream stream, ulong value)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian == true)
            {
                Array.Reverse(bytes);
            }
            stream.Write(bytes, 0, bytes.Length);
        }

        public override object Clone()
        {
            return new TagULong(this.value);
        }

        public static explicit operator TagULong(ulong value)
        {
            return new TagULong(value);
        }

        public override Type getType()
        {
            return typeof(TagULong);
        }

        public bool Equals(TagULong other)
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

            if (typeof(TagULong) != other.getType())
            {
                bResult = false;
            }
            else
            {
                bResult = this.Equals((TagULong)other);
            }

            return bResult;
        }
    }
}
