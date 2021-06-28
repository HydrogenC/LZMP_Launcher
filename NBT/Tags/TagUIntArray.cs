using System;
using System.IO;
using System.Linq;
using NBT.Exceptions;

namespace NBT.Tags
{
    public sealed class TagUIntArray : Tag, IEquatable<TagUIntArray>
    {
        public uint[] value;

        public TagUIntArray() : this(new uint[0])
        {
        }

        public TagUIntArray(uint[] value)
        {
            if (value == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            this.value = value;
        }
        
        internal TagUIntArray(Stream stream) : this(new uint[0])
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
                if (value.GetType() != typeof(uint[]))
                {
                    throw new NBT_InvalidArgumentException();
                }
                this.value = (uint[])value;
            }
        }

        public override byte tagID
        {
            get
            {
                return TagTypes.TagUIntArray;            
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
            this.value = TagUIntArray.ReadUIntegerArray(stream);
        }

        internal override void writeTag(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            TagUIntArray.WriteUIntegerArray(stream, this.value);
        }

        internal static uint[] ReadUIntegerArray(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            uint[] buffer = new uint[TagInt.ReadInt(stream)];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = TagUInt.ReadUInt(stream);
            }
            return buffer;
        }

        internal static void WriteUIntegerArray(Stream stream, uint[] value)
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
                    TagUInt.WriteUInt(stream, value[i]);
                }
            }
        }

        public override object Clone()
        {
            return new TagUIntArray(this.value);
        }

        public static explicit operator TagUIntArray(uint[] value)
        {
            return new TagUIntArray(value);
        }

        public override Type getType()
        {
            return typeof(TagUIntArray);
        }

        public bool Equals(TagUIntArray other)
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

            if (typeof(TagUIntArray) != other.getType())
            {
                bResult = false;
            }
            else
            {
                bResult = this.Equals((TagUIntArray)other);
            }

            return bResult;
        }
    }
}
