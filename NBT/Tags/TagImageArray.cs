using System;
using System.IO;
using System.Linq;
using System.Drawing;
using NBT.Exceptions;

namespace NBT.Tags
{
    public sealed class TagImageArray : Tag, IEquatable<TagImageArray>
    {
        public Image[] value;

        public TagImageArray() : this(new Image[0])
        { 
        }

        public TagImageArray(Image[] value)
        {
            if (value == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            this.value = value;
        }

        internal TagImageArray(Stream stream) : this(new Image[0])
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
                if (value.GetType() != typeof(Image[]))
                {
                    throw new NBT_InvalidArgumentException();
                }
                this.value = (Image[])value;
            }
        }

        public override byte tagID
        {
            get 
            {
                return TagTypes.TagImageArray;
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
            this.value = TagImageArray.ReadImageArray(stream);
        }

        internal override void writeTag(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            TagImageArray.WriteImageArray(stream, this.value);
        }

        internal static Image[] ReadImageArray(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            Image[] buffer = new Image[TagInt.ReadInt(stream)];
            for (int i = 0; i < buffer.Length; i++) 
            {
                buffer[i] = TagImage.ReadImage(stream);
            }
            return buffer;
        }

        internal static void WriteImageArray(Stream stream, Image[] value) 
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
                    TagImage.WriteImage(stream, value[i]);
                }            
            }
        }

        public override object Clone()
        {
            return new TagImageArray(this.value);
        }

        public static explicit operator TagImageArray(Image[] value) 
        {
            return new TagImageArray(value);
        }

        public override Type getType()
        {
            return typeof(TagImageArray);
        }

        public bool Equals(TagImageArray other)
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

            if (typeof(TagImageArray) != other.getType())
            {
                bResult = false;
            }
            else
            {
                bResult = this.Equals((TagImageArray)other);
            }

            return bResult;
        }
    }
}
