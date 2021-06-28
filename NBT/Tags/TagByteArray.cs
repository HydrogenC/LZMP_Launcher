using System;
using System.IO;
using System.Linq;
using NBT.Exceptions;

namespace NBT.Tags
{
    public sealed class TagByteArray : Tag, IEquatable<TagByteArray>
    {
        public byte[] value;

        public TagByteArray() : this(new byte[0])
        {
        }

        public TagByteArray(byte[] value)
        {
            if (value == null) 
            {
                throw new NBT_InvalidArgumentNullException();
            }
            this.value = value;
        }
        
        internal TagByteArray(Stream stream) : this(new byte[0])
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
                if (value.GetType() != typeof(byte[]))
                {
                    throw new NBT_InvalidArgumentException();
                }
                this.value = (byte[])value;
            }
        }

        public override byte tagID
        {
            get
            {
                return TagTypes.TagByteArray;            
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
            this.value = TagByteArray.ReadByteArray(stream);
        }

        internal override void writeTag(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            TagByteArray.WriteByteArray(stream, this.value);
        }

        internal static byte[] ReadByteArray(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            byte[] buffer = new byte[TagInt.ReadInt(stream)];
            if (stream.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                throw new NBT_EndOfStreamException();
            }
            return buffer;
        }

        internal static void WriteByteArray(Stream stream, byte[] value)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            if (value == null)
            {
                TagInt.WriteInt(stream, 0);
            }
            else
            { 
                TagInt.WriteInt(stream, value.Length);
                stream.Write(value, 0, value.Length);            
            }
        }

        public override object Clone()
        {
            return new TagByteArray(this.value);
        }

        public static explicit operator TagByteArray(byte[] value)
        {
            return new TagByteArray(value);
        }

        public override Type getType()
        {
            return typeof(TagByteArray);
        }

        public bool Equals(TagByteArray other)
        {
            bool bResult = false;
            try
            {
                bResult = this.value.SequenceEqual(other.value);
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

            if (typeof(TagByteArray) != other.getType())
            {
                bResult = false;
            }
            else
            {
                bResult = this.Equals((TagByteArray)other);
            }

            return bResult;
        }
    }
}
