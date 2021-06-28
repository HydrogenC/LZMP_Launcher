using System;
using System.IO;
using System.Linq;
using NBT.Exceptions;

namespace NBT.Tags
{
    public sealed class TagULongArray : Tag, IEquatable<TagULongArray>
    {
        public ulong[] value;

        public TagULongArray() : this(new ulong[0])
        {
        }

        public TagULongArray(ulong[] value)
        {
            if (value == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            this.value = value;
        }
        
        internal TagULongArray(Stream stream) : this(new ulong[0])
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
                if (value.GetType() != typeof(ulong[]))
                {
                    throw new NBT_InvalidArgumentException();
                }
                this.value = (ulong[])value;
            }
        }

        public override byte tagID
        {
            get
            {
                return TagTypes.TagULongArray;            
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
            this.value = TagULongArray.ReadULongArray(stream);
        }

        internal override void writeTag(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            TagULongArray.WriteULongArray(stream, this.value);
        }

        internal static ulong[] ReadULongArray(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            ulong[] buffer = new ulong[TagInt.ReadInt(stream)];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = TagULong.ReadULong(stream);
            }
            return buffer;
        }

        internal static void WriteULongArray(Stream stream, ulong[] value)
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
                    TagULong.WriteULong(stream, value[i]);
                }
            }
        }

        public override object Clone()
        {
            return new TagULongArray(this.value);
        }

        public static explicit operator TagULongArray(ulong[] value)
        {
            return new TagULongArray(value);
        }

        public override Type getType()
        {
            return typeof(TagULongArray);
        }

        public bool Equals(TagULongArray other)
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

            if (typeof(TagULongArray) != other.getType())
            {
                bResult = false;
            }
            else
            {
                bResult = this.Equals((TagULongArray)other);
            }

            return bResult;
        }
    }
}
