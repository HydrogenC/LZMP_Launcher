using System;
using System.IO;
using System.Text;
using NBT.Exceptions;

namespace NBT.Tags
{
    public sealed class TagString : Tag, IEquatable<TagString>
    {

        public string value;

        public TagString() : this("")
        {
        }

        public TagString(string value)
        {
            this.value = value;
        }

        internal TagString(Stream stream) : this("")
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
                return TagTypes.TagString;
            }
        }

        public override string toString()
        {
            return this.value;
        }

        internal override void readTag(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            this.value = TagString.ReadString(stream);
        }

        internal override void writeTag(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            TagString.WriteString(stream, this.value);
        }

        internal static string ReadString(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            byte[] buffer = new byte[TagUShort.ReadUShort(stream)];
            if (stream.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                throw new NBT_EndOfStreamException();
            }
            return Encoding.UTF8.GetString(buffer);
        }

        internal static void WriteString(Stream stream, string value)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            if (Encoding.UTF8.GetByteCount(value) > ushort.MaxValue)
            {
                throw new NBT_InvalidArgumentException("String is too long");
            }
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            TagUShort.WriteUShort(stream, (ushort)bytes.Length);
            stream.Write(bytes, 0, bytes.Length);
        }

        public override object Clone()
        {
            return new TagString(this.value);
        }

        public static explicit operator TagString(string value)
        {
            return new TagString(value);
        }

        public override Type getType()
        {
            return typeof(TagString);
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
                if (value.GetType() != typeof(string))
                {
                    throw new NBT_InvalidArgumentException();
                }
                this.value = (string)value;
            }
        }

        public bool Equals(TagString other)
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

            if (typeof(TagString) != other.getType())
            {
                bResult = false;
            }
            else
            {
                bResult = this.Equals((TagString)other);
            }

            return bResult;
        }
    }
}
