using System;
using System.IO;
using System.Linq;
using NBT.Exceptions;

namespace NBT.Tags
{
    public sealed class TagIntArray : Tag, IEquatable<TagIntArray>
    {
        public int[] value;

        public TagIntArray() : this(new int[0])
        {
        }

        public TagIntArray(int[] value)
        {
            if (value == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            this.value = value;
        }
        
        internal TagIntArray(Stream stream) : this(new int[0])
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
                if (value.GetType() != typeof(int[]))
                {
                    throw new NBT_InvalidArgumentException();
                }
                this.value = (int[])value;
            }
        }

        public override byte tagID
        {
            get
            {
                return TagTypes.TagIntArray;            
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
            this.value = TagIntArray.ReadIntegerArray(stream);
        }

        internal override void writeTag(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            TagIntArray.WriteIntegerArray(stream, this.value);
        }

        internal static int[] ReadIntegerArray(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            int[] buffer = new int[TagInt.ReadInt(stream)];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = TagInt.ReadInt(stream);
            }
            return buffer;
        }

        internal static void WriteIntegerArray(Stream stream, int[] value)
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
                    TagInt.WriteInt(stream, value[i]);
                }            
            }
        }

        public override object Clone()
        {
            return new TagIntArray(this.value);
        }

        public static explicit operator TagIntArray(int[] value)
        {
            return new TagIntArray(value);
        }

        public override Type getType()
        {
            return typeof(TagIntArray);
        }

        public bool Equals(TagIntArray other)
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

            if (typeof(TagIntArray) != other.getType())
            {
                bResult = false;
            }
            else
            {
                bResult = this.Equals((TagIntArray)other);
            }

            return bResult;
        }
    }
}
