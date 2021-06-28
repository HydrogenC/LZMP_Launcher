using System;
using System.IO;
using NBT.Exceptions;

namespace NBT.Tags
{
    public sealed class TagLong : Tag, IEquatable<TagLong>
    {
        public long value;

        public TagLong() : this((long)0)
        {
        }

        public TagLong(long value)
        {
            this.value = value;
        }

        internal TagLong(Stream stream) : this((long)0)
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
                if (value.GetType() != typeof(long))
                {
                    throw new NBT_InvalidArgumentException();
                }
                this.value = (long)value;
            }
        }

        public override byte tagID
        {
            get
            {
                return TagTypes.TagLong;
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
            this.value = TagLong.ReadLong(stream);
        }

        internal override void writeTag(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            TagLong.WriteLong(stream, this.value);
        }

        internal static long ReadLong(Stream stream)
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
            return BitConverter.ToInt64(buffer, 0);
        }

        internal static void WriteLong(Stream stream, long value)
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
            return new TagLong(this.value);
        }

        public static explicit operator TagLong(long value)
        {
            return new TagLong(value);
        }

        public override Type getType()
        {
            return typeof(TagLong);
        }

        public bool Equals(TagLong other)
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

            if (typeof(TagLong) != other.getType())
            {
                bResult = false;
            }
            else
            {
                bResult = this.Equals((TagLong)other);
            }

            return bResult;
        }
    }
}
