using System;
using System.IO;
using System.Linq;
using NBT.Exceptions;

namespace NBT.Tags
{
    public sealed class TagShortArray : Tag, IEquatable<TagShortArray>
    {
        public short[] value;

        public TagShortArray() : this(new short[0])
        { 
        }

        public TagShortArray(short[] value)
        {
            if (value == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            this.value = value;
        }

        internal TagShortArray(Stream stream) : this(new short[0])
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
                if (value.GetType() != typeof(short[]))
                {
                    throw new NBT_InvalidArgumentException();
                }
                this.value = (short[])value;
            }
        }

        public override byte tagID
        {
            get 
            {
                return TagTypes.TagShortArray;
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
            this.value = TagShortArray.ReadShortArray(stream);
        }

        internal override void writeTag(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            TagShortArray.WriteShortArray(stream, this.value);
        }

        internal static short[] ReadShortArray(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            short[] buffer = new short[TagInt.ReadInt(stream)];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = TagShort.ReadShort(stream);
            }
            return buffer;
        }

        internal static void WriteShortArray(Stream stream, short[] value)
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
                    TagShort.WriteShort(stream, value[i]);
                }            
            }
        }

        public override object Clone()
        {
            return new TagShortArray(this.value);
        }

        public static explicit operator TagShortArray(short[] value)
        {
            return new TagShortArray(value);        
        }

        public override Type getType()
        {
            return typeof(TagShortArray);
        }

        public bool Equals(TagShortArray other)
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

            if (typeof(TagShortArray) != other.getType())
            {
                bResult = false;
            }
            else
            {
                bResult = this.Equals((TagShortArray)other);
            }

            return bResult;
        }
    }
}
