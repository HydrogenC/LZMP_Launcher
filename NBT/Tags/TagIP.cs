using System;
using System.IO;
using System.Net;
using NBT.Exceptions;

namespace NBT.Tags
{
    public sealed class TagIP : Tag, IEquatable<TagIP>
    {
        public IPAddress value;

        public TagIP() : this(new IPAddress(new byte[4] { 127, 0, 0, 1 }))
        {
        }

        public TagIP(IPAddress value)
        {
            this.value = value;
        }

        internal TagIP(Stream stream) : this(new IPAddress(new byte[4] { 127, 0, 0, 1 }))
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
                if (value.GetType() != typeof(IPAddress))
                {
                    throw new NBT_InvalidArgumentException();
                }
                this.value = (IPAddress)value;
            }
        }

        public override byte tagID
        {
            get
            {
                return TagTypes.TagIP;
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
            this.value = TagIP.ReadIP(stream);
        }

        internal override void writeTag(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            TagIP.WriteIP(stream, this.value);
        }

        internal static IPAddress ReadIP(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            return new IPAddress(TagByteArray.ReadByteArray(stream));
        }

        internal static void WriteIP(Stream stream, IPAddress value)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            if (value == null)
            {
                TagByteArray.WriteByteArray(stream, new byte[] { 127, 0, 0, 1 });
            }
            else
            { 
                TagByteArray.WriteByteArray(stream, value.GetAddressBytes());            
            }
        }

        public override object Clone()
        {
            return new TagIP(this.value);
        }

        public static explicit operator TagIP(IPAddress value)
        {
            return new TagIP(value);
        }

        public override Type getType()
        {
            return typeof(TagIP);
        }

        public bool Equals(TagIP other)
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

            if (typeof(TagIP) != other.getType())
            {
                bResult = false;
            }
            else
            {
                bResult = this.Equals((TagIP)other);
            }

            return bResult;
        }
    }
}
