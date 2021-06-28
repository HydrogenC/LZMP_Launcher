using System;
using System.IO;
using NBT.Exceptions;

namespace NBT.Tags
{
    public sealed class TagTimeSpan : Tag, IEquatable<TagTimeSpan>
    {
        public TimeSpan value;

        public TagTimeSpan() : this(new TimeSpan(DateTime.Now.Ticks))
        { 
        }

        public TagTimeSpan(TimeSpan value)
        {
            this.value = value;
        }

        internal TagTimeSpan(Stream stream) : this(new TimeSpan(DateTime.Now.Ticks))
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
                if (value.GetType() != typeof(TimeSpan))
                {
                    throw new NBT_InvalidArgumentException();
                }
                this.value = (TimeSpan)value;
            }
        }

        public override byte tagID
        {
            get 
            {
                return TagTypes.TagTimeSpan;
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
            this.value = TagTimeSpan.ReadTimeSpan(stream);
        }

        internal override void writeTag(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            TagTimeSpan.WriteTimeSpan(stream, this.value);
        }

        internal static TimeSpan ReadTimeSpan(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            return new TimeSpan(TagLong.ReadLong(stream));
        }

        internal static void WriteTimeSpan(Stream stream, TimeSpan value)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            TagLong.WriteLong(stream, value.Ticks); 
        }

        public override object Clone()
        {
            return new TagTimeSpan(this.value);
        }

        public static explicit operator TagTimeSpan(TimeSpan value)
        {
            return new TagTimeSpan(value);
        }

        public override Type getType()
        {
            return typeof(TagTimeSpan);
        }

        public bool Equals(TagTimeSpan other)
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

            if (typeof(TagTimeSpan) != other.getType())
            {
                bResult = false;
            }
            else
            {
                bResult = this.Equals((TagTimeSpan)other);
            }

            return bResult;
        }
    }
}
