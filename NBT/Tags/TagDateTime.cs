using System;
using System.IO;
using NBT.Exceptions;

namespace NBT.Tags
{
    public sealed class TagDateTime : Tag, IEquatable<TagDateTime>
    {
        public DateTime value;

        public TagDateTime() : this(new DateTime(DateTime.Now.Ticks))
        { 
        }

        public TagDateTime(DateTime value)
        {
            this.value = value;
        }

        internal TagDateTime(Stream stream): this(new DateTime(DateTime.Now.Ticks))
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
                if (value.GetType() != typeof(DateTime))
                {
                    throw new NBT_InvalidArgumentException();
                }
                this.value = (DateTime)value;
            }
        }

        public override byte tagID
        {
            get 
            {
                return TagTypes.TagDateTime;
            }
        }

        public override string toString()
        {
            return this.value.ToShortDateString();
        }

        internal override void readTag(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            this.value = TagDateTime.ReadDateTime(stream);
        }

        internal override void writeTag(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            TagDateTime.WriteDateTime(stream, this.value);
        }

        internal static DateTime ReadDateTime(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            return new DateTime(TagLong.ReadLong(stream));
        }

        internal static void WriteDateTime(Stream stream, DateTime value)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            TagLong.WriteLong(stream, value.Ticks);
        }

        public override object Clone()
        {
            return new TagDateTime(this.value);
        }

        public static explicit operator TagDateTime(DateTime value) 
        {
            return new TagDateTime(value); 
        }

        public override Type getType()
        {
            return typeof(TagDateTime);
        }

        public bool Equals(TagDateTime other)
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

            if (typeof(TagDateTime) != other.getType())
            {
                bResult = false;
            }
            else
            {
                bResult = this.Equals((TagDateTime)other);
            }

            return bResult;
        }
    }
}
