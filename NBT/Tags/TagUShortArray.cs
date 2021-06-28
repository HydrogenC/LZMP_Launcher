using System;
using System.IO;
using System.Linq;
using NBT.Exceptions;

namespace NBT.Tags
{
    public sealed class TagUShortArray : Tag, IEquatable<TagUShortArray>
    {
        public ushort[] value;

        public TagUShortArray() : this(new ushort[0])
        { 
        }

        public TagUShortArray(ushort[] value)
        {
            if (value == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            this.value = value;
        }

        internal TagUShortArray(Stream stream) : this(new ushort[0])
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
                if (value.GetType() != typeof(ushort[]))
                {
                    throw new NBT_InvalidArgumentException();
                }
                this.value = (ushort[])value;
            }
        }

        public override byte tagID
        {
            get 
            {
                return TagTypes.TagUShortArray;
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
            this.value = TagUShortArray.ReadUShortArray(stream);
        }

        internal override void writeTag(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            TagUShortArray.WriteUShortArray(stream, this.value);
        }

        internal static ushort[] ReadUShortArray(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            ushort[] buffer = new ushort[TagInt.ReadInt(stream)];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = TagUShort.ReadUShort(stream);
            }
            return buffer;
        }

        internal static void WriteUShortArray(Stream stream, ushort[] value)
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
                for (int i = 0; i < value.Length; i++)
                {
                    TagUShort.WriteUShort(stream, value[i]);
                }
            }
        }

        public override object Clone()
        {
            return new TagUShortArray(this.value);
        }

        public static explicit operator TagUShortArray(ushort[] value)
        {
            return new TagUShortArray(value);        
        }

        public override Type getType()
        {
            return typeof(TagUShortArray);
        }

        public bool Equals(TagUShortArray other)
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

            if (typeof(TagUShortArray) != other.getType())
            {
                bResult = false;
            }
            else
            {
                bResult = this.Equals((TagUShortArray)other);
            }

            return bResult;
        }
    }
}
