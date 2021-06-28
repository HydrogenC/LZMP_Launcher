using System;
using System.IO;
using System.Linq;
using NBT.Exceptions;

namespace NBT.Tags
{
    public sealed class TagDoubleArray : Tag, IEquatable<TagDoubleArray>
    {
        public double[] value;

        public TagDoubleArray() : this(new double[0]) 
        {
        }

        public TagDoubleArray(double[] value) 
        {
            if (value == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            this.value = value;
        }

        internal TagDoubleArray(Stream stream)  : this(new double[0])
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
                if (value.GetType() != typeof(double[]))
                {
                    throw new NBT_InvalidArgumentException();
                }
                this.value = (double[])value;
            }
        }

        public override byte tagID
        {
            get 
            {
                return TagTypes.TagDoubleArray; 
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
            this.value = TagDoubleArray.ReadDoubleArray(stream);
        }

        internal override void writeTag(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            TagDoubleArray.WriteDoubleArray(stream, this.value);
        }

        internal static double[] ReadDoubleArray(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            double[] buffer = new double[TagInt.ReadInt(stream)];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = TagDouble.ReadDouble(stream);
            }
            return buffer;
        }

        internal static void WriteDoubleArray(Stream stream, double[] value) 
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
                    TagDouble.WriteDouble(stream, value[i]);
                }            
            }
        }

        public override object Clone()
        {
            return new TagDoubleArray(this.value);
        }

        public static explicit operator TagDoubleArray(double[] value) 
        {
            return new TagDoubleArray(value);
        }

        public override Type getType()
        {
            return typeof(TagDoubleArray);
        }

        public bool Equals(TagDoubleArray other)
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

            if (typeof(TagDoubleArray) != other.getType())
            {
                bResult = false;
            }
            else
            {
                bResult = this.Equals((TagDoubleArray)other);
            }

            return bResult;
        }
    }
}
