using System;
using System.IO;
using System.Drawing;
using NBT.Exceptions;

namespace NBT.Tags
{
    public sealed class TagImage : Tag, IEquatable<TagImage>
    {
        public Image value;

        public TagImage()
        {
        }

        public TagImage(Image value)
        {
            this.value = value;
        }

        internal TagImage(Stream stream)
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
                if (value.GetType() != typeof(Image))
                {
                    throw new NBT_InvalidArgumentException();
                }
                this.value = (Image)value;
            }
        }

        public override byte tagID
        {
            get
            {
                return TagTypes.TagImage;
            }
        }

        public override string toString()
        {
            return "";
        }

        internal override void readTag(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            this.value = TagImage.ReadImage(stream);
        }

        internal override void writeTag(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            TagImage.WriteImage(stream, this.value);
        }

        internal static Image ReadImage(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            byte[] buffer = new byte[TagInt.ReadInt(stream)];
            if (stream.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                throw new NBT_EndOfStreamException();
            }
            if (buffer.Length > 0)
            {
                MemoryStream ms = new MemoryStream(buffer, 0, buffer.Length);
                return Image.FromStream(ms);
            }
            else
            {
                return null;
            }
        }

        internal static void WriteImage(Stream stream, Image value)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            if (value != null)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    Bitmap temporalImage = new Bitmap(value);
                    temporalImage.Save(memoryStream, value.RawFormat);
                    temporalImage.Dispose();
                    byte[] bytes = memoryStream.GetBuffer();
                    TagInt.WriteInt(stream, bytes.Length);
                    stream.Write(bytes, 0, bytes.Length);
                }
            }
            else
            {
                TagInt.WriteInt(stream, 0);
            }
        }

        public override object Clone()
        {
            return new TagImage(this.value);
        }

        public static explicit operator TagImage(Image value)
        {
            return new TagImage(value);
        }

        public override Type getType()
        {
            return typeof(TagImage);
        }

        public bool Equals(TagImage other)
        {
            bool bResult = false;
            try
            {
                bResult = this.value.Equals(other.value);
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

            if (typeof(TagImage) != other.getType())
            {
                bResult = false;
            }
            else
            {
                bResult = this.Equals((TagImage)other);
            }

            return bResult;
        }
    }
}
