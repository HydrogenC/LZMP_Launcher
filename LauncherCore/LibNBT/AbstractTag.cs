using System;
using System.IO;
using System.IO.Compression;

namespace LibNBT
{
    public enum TagType { End = 0, Byte = 1, Short = 2, Int = 3, Long = 4, Float = 5, Double = 6, ByteArray = 7, String = 8, List = 9, Compound = 10, IntArray = 11 };

    public abstract class AbstractTag
    {
        public abstract TagType Type { get; }
        public virtual string Name { get; set; }

        public abstract void Write(Stream output);
        public abstract void WriteUnnamed(Stream output);

        public static AbstractTag Read(Stream input)
        {
            int temp = input.ReadByte();
            if (temp != (temp & 0xFF))
            {
                throw new Exception();
            }

            switch ((TagType)temp)
            {
                case TagType.End:
                    return new TagEnd();
                case TagType.Byte:
                    return new TagByte(input);
                case TagType.Short:
                    return new TagShort(input);
                case TagType.Int:
                    return new TagInt(input);
                case TagType.IntArray:
                    return new TagIntArray(input);
                case TagType.Long:
                    return new TagLong(input);
                case TagType.Float:
                    return new TagFloat(input);
                case TagType.Double:
                    return new TagDouble(input);
                case TagType.ByteArray:
                    return new TagByteArray(input);
                case TagType.String:
                    return new TagString(input);
                case TagType.List:
                    return new TagList(input);
                case TagType.Compound:
                    return new TagCompound(input);
                default:
                    throw new NotImplementedException();
            }
        }

        public static AbstractTag ReadFromGzippedFile(string filename)
        {
            using (FileStream input = File.OpenRead(filename))
            {
                using (GZipStream gzipStream = new GZipStream(input, CompressionMode.Decompress))
                {
                    return AbstractTag.Read(gzipStream);
                }
            }
        }

        public static AbstractTag ReadFromFile(string filename)
        {
            AbstractTag tag = null;
            //Check if gzipped stream
            try
            {
                using (FileStream input = File.OpenRead(filename))
                {
                    using (GZipStream gzipStream = new GZipStream(input, CompressionMode.Decompress))
                    {
                        tag = AbstractTag.Read(gzipStream);
                    }
                }
            }
            catch (Exception)
            {
                tag = null;
            }

            if (tag != null)
            {
                return tag;
            }

            //Try Deflate stream
            try
            {
                using (FileStream input = File.OpenRead(filename))
                {
                    using (DeflateStream deflateStream = new DeflateStream(input, CompressionMode.Decompress))
                    {
                        tag = AbstractTag.Read(deflateStream);
                    }
                }
            }
            catch (Exception)
            {
                tag = null;
            }

            if (tag != null)
            {
                return tag;
            }

            //Assume uncompressed stream
            using (FileStream input = File.OpenRead(filename))
            {
                tag = AbstractTag.Read(input);
            }

            return tag;
        }

        public override string ToString()
        {
            return ToString(string.Empty);
        }

        public abstract string ToString(string indentString);
    }
}
