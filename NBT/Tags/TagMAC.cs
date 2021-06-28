using System;
using System.IO;
using NBT.Exceptions;
using System.Net.NetworkInformation;

namespace NBT.Tags
{
    public sealed class TagMAC : Tag, IEquatable<TagMAC>
    {
        public PhysicalAddress value;

        public TagMAC() : this(new PhysicalAddress(new byte[6] { 0, 0, 0, 0, 0, 0 }))
        {
        }

        public TagMAC(PhysicalAddress value)
        {
            this.value = value;
        }

        internal TagMAC(Stream stream) : this(new PhysicalAddress(new byte[6] { 0, 0, 0, 0, 0, 0 }))
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
                if (value.GetType() != typeof(PhysicalAddress))
                {
                    throw new NBT_InvalidArgumentException();
                }
                this.value = (PhysicalAddress)value;
            }
        }

        public override byte tagID
        {
            get
            {
                return TagTypes.TagMAC;
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
            this.value = TagMAC.ReadMAC(stream);
        }

        internal override void writeTag(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            TagMAC.WriteMAC(stream, this.value);
        }

        internal static PhysicalAddress ReadMAC(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            byte[] buffer = new byte[6];
            if (stream.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                throw new NBT_EndOfStreamException();
            }
            if (BitConverter.IsLittleEndian == true) 
            {
                Array.Reverse(buffer);            
            }
            return new PhysicalAddress(buffer);
        }
        
        internal static void WriteMAC(Stream stream, PhysicalAddress value)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            byte[] bytes;
            if (value == null)
            {
                bytes = new byte[] { 0, 0, 0, 0, 0, 0 };
            }
            else
            { 
                bytes = value.GetAddressBytes();            
            }
            if (BitConverter.IsLittleEndian == true)
            { 
                Array.Reverse(bytes);            
            }
            stream.Write(bytes, 0, bytes.Length);  
        }

        public override object Clone()
        {
            return new TagMAC(this.value);
        }

        public static explicit operator TagMAC(PhysicalAddress value)
        {
            return new TagMAC(value);
        }

        public override Type getType()
        {
            return typeof(TagMAC);
        }

        public bool Equals(TagMAC other)
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

            if (typeof(TagMAC) != other.getType())
            {
                bResult = false;
            }
            else
            {
                bResult = this.Equals((TagMAC)other);
            }

            return bResult;
        }
    }
}
