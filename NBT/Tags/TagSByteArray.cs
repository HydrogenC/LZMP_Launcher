using System;
using System.IO;
using System.Linq;
using NBT.Exceptions;

namespace NBT.Tags
{
    public sealed class TagSByteArray : Tag, IEquatable<TagSByteArray>
    {

        public sbyte[] value;

        public TagSByteArray() : this(new sbyte[0])
        {
        }

        public TagSByteArray(sbyte[] value)
        {
            if (value == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            this.value = value;
        }

        internal TagSByteArray(Stream stream) : this(new sbyte[0])
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
                if (value.GetType() != typeof(sbyte[]))
                {
                    throw new NBT_InvalidArgumentException();
                }
                this.value = (sbyte[])value;
            }
        }

        public override byte tagID
        {
            get 
            {
                return TagTypes.TagSByteArray;
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
            this.value = TagSByteArray.ReadSByteArray(stream);
        }

        internal override void writeTag(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            TagSByteArray.WriteSByteArray(stream, this.value);
        }

        internal static sbyte[] ReadSByteArray(Stream stream) 
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            sbyte[] buffer = new sbyte[TagInt.ReadInt(stream)];
            for (int i = 0; i < buffer.Length; i++) 
            {
                buffer[i] = TagSByte.ReadSByte(stream);
            }
            return buffer;
        }

        internal static void WriteSByteArray(Stream stream, sbyte[] value)
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
                    TagSByte.WriteSByte(stream, value[i]);
                }            
            }
        }

        public override object Clone()
        {
            return new TagSByteArray(this.value);
        }

        public static explicit operator TagSByteArray(sbyte[] value)
        {
            return new TagSByteArray(value);
        }

        public override Type getType()
        {
            return typeof(TagSByteArray);
        }

        public bool Equals(TagSByteArray other)
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

            if (typeof(TagSByteArray) != other.getType())
            {
                bResult = false;
            }
            else
            {
                bResult = this.Equals((TagSByteArray)other);
            }

            return bResult;
        }
    }
}
