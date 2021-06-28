using System;
using System.IO;
using NBT.Exceptions;

namespace NBT.Tags
{
    public sealed class TagUShort : Tag, IEquatable<TagUShort>
    {
        public ushort value;

        public TagUShort() : this((ushort)0)
        {
        }

        public TagUShort(ushort value)
        {
            this.value = value;
        }

        internal TagUShort(Stream stream) : this((ushort)0)
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
                if (value.GetType() != typeof(ushort))
                {
                    throw new NBT_InvalidArgumentException();
                }
                this.value = (ushort)value;
            }
        }

        public override byte tagID
        {
            get
            {
                return TagTypes.TagUShort;
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
            this.value = TagUShort.ReadUShort(stream);
        }

        internal override void writeTag(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            TagUShort.WriteUShort(stream, this.value);
        }

        internal static ushort ReadUShort(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            byte[] buffer = new byte[2];
            if (stream.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                throw new NBT_EndOfStreamException();
            }
            if (BitConverter.IsLittleEndian == true)
            { 
                Array.Reverse(buffer);            
            }
            return BitConverter.ToUInt16(buffer, 0);
        }

        internal static void WriteUShort(Stream stream, ushort value)
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
            return new TagUShort(this.value);
        }

        public static explicit operator TagUShort(ushort value)
        {
            return new TagUShort(value);
        }

        public override Type getType()
        {
            return typeof(TagUShort);
        }

        public bool Equals(TagUShort other)
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

            if (typeof(TagUShort) != other.getType())
            {
                bResult = false;
            }
            else
            {
                bResult = this.Equals((TagUShort)other);
            }

            return bResult;
        }
    }
}
