using System;
using System.IO;
using NBT.Exceptions;

namespace NBT.Tags
{
    public sealed class TagEnd : Tag
    {

        public TagEnd()
        { 
        
        }

        public override byte tagID
        {
            get
            {
                return TagTypes.TagEnd;
            }
        }

        public override string toString()
        {
            return "";
        }

        internal override void readTag(Stream stream)
        {
            throw new NotImplementedException();
        }

        internal override void writeTag(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            stream.WriteByte(TagTypes.TagEnd);
        }

        public override object Clone()
        {
            return new TagEnd();
        }

        public override Type getType()
        {
            return typeof(TagEnd);
        }

        public override object Value
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override bool Equals(Tag other)
        {
            bool bResult = true;

            if (typeof(TagEnd) != other.getType())
            {
                bResult = false;
            }

            return bResult;
        }
    }
}
