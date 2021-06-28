using System;
using System.IO;
using System.Linq;
using NBT.Exceptions;

namespace NBT.Tags
{
    public sealed class TagFloatArray : Tag, IEquatable<TagFloatArray>
    {
        public float[] value;

        public TagFloatArray() : this(new float[0])
        { 
        }

        public TagFloatArray(float[] value)
        {
            if (value == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            this.value = value;
        }

        internal TagFloatArray(Stream stream) : this(new float[0])
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
                if (value.GetType() != typeof(float[]))
                {
                    throw new NBT_InvalidArgumentException();
                }
                this.value = (float[])value;
            }
        }

        public override byte tagID
        {
            get 
            {
                return TagTypes.TagFloatArray;
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
            this.value = TagFloatArray.ReadFloatArray(stream);
        }

        internal override void writeTag(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            TagFloatArray.WriteFloatArray(stream, this.value);
        }

        internal static float[] ReadFloatArray(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            float[] buffer = new float[TagInt.ReadInt(stream)];
            for (int i = 0; i < buffer.Length; i++) 
            {
                buffer[i] = TagFloat.ReadFloat(stream);
            }
            return buffer;
        }

        internal static void WriteFloatArray(Stream stream, float[] value) 
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
                    TagFloat.WriteFloat(stream, value[i]);
                }            
            }
        }

        public override object Clone()
        {
            return new TagFloatArray(this.value);
        }

        public static explicit operator TagFloatArray(float[] value) 
        {
            return new TagFloatArray(value);
        }

        public override Type getType()
        {
            return typeof(TagFloatArray);
        }

        public bool Equals(TagFloatArray other)
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

            if (typeof(TagFloatArray) != other.getType())
            {
                bResult = false;
            }
            else
            {
                bResult = this.Equals((TagFloatArray)other);
            }

            return bResult;
        }
    }
}
