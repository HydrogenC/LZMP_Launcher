using System;
using System.IO;
using NBT.Exceptions;

namespace NBT.Tags
{
    public sealed class TagUInt : Tag, IEquatable<TagUInt>
    {
        public uint value;

        public TagUInt() : this((uint)0)
        {
        }

        public TagUInt(uint value)
        {
            this.value = value;
        }

        internal TagUInt(Stream stream) : this((uint)0)
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
                if (value.GetType() != typeof(uint))
                {
                    throw new NBT_InvalidArgumentException();
                }
                this.value = (uint)value;
            }
        }

        public override byte tagID
        {
            get 
            {
                return TagTypes.TagUInt; 
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
            this.value = TagUInt.ReadUInt(stream);
        }

        internal override void writeTag(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            TagUInt.WriteUInt(stream, this.value);
        }

        internal static uint ReadUInt(Stream stream) 
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            byte[] buffer = new byte[4];
            if (stream.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                throw new NBT_EndOfStreamException();
            }
            if (BitConverter.IsLittleEndian == true)
            {
                Array.Reverse(buffer);
            }
            return BitConverter.ToUInt32(buffer, 0);
        }

        internal static void WriteUInt(Stream stream, uint value)
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
            return new TagUInt(this.value);
        }

        public static explicit operator TagUInt(uint value)
        {
            return new TagUInt(value);
        }

        public override Type getType()
        {
            return typeof(TagUInt);
        }

        public bool Equals(TagUInt other)
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

            if (typeof(TagUInt) != other.getType())
            {
                bResult = false;
            }
            else
            {
                bResult = this.Equals((TagUInt)other);
            }

            return bResult;
        }
    }
}
