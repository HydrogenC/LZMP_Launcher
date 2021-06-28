using System;
using System.IO;
using System.Linq;
using NBT.Exceptions;

namespace NBT.Tags
{
    public sealed class TagLongArray : Tag, IEquatable<TagLongArray>
    {
        public long[] value;

        public TagLongArray() : this(new long[0]) 
        { 
        }

        public TagLongArray(long[] value) 
        {
            if (value == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            this.value = value;
        }

        internal TagLongArray(Stream stream) : this(new long[0])
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
                if (value.GetType() != typeof(long[])) 
                {
                    throw new NBT_InvalidArgumentException();
                }
                this.value = (long[])value;
            }
        }

        public override byte tagID
        {
            get 
            {
                return TagTypes.TagLongArray; 
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
            this.value = TagLongArray.ReadLongArray(stream);
        }

        internal override void writeTag(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            TagLongArray.WriteLongArray(stream, this.value);
        }

        internal static long[] ReadLongArray(Stream stream) 
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            long[] buffer = new long[TagInt.ReadInt(stream)];
            for (int i = 0; i < buffer.Length; i++) 
            {
                buffer[i] = TagLong.ReadLong(stream);
            }
            return buffer;
        }

        internal static void WriteLongArray(Stream stream, long[] value) 
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
                    TagLong.WriteLong(stream, value[i]);
                }            
            }
        }

        public override object Clone()
        {
            return new TagLongArray(this.value);
        }

        public static explicit operator TagLongArray(long[] value) 
        {
            return new TagLongArray(value);
        }

        public override Type getType()
        {
            return typeof(TagLongArray);
        }

        public bool Equals(TagLongArray other)
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

            if (typeof(TagLongArray) != other.getType())
            {
                bResult = false;
            }
            else
            {
                bResult = this.Equals((TagLongArray)other);
            }

            return bResult;
        }
    }
}
