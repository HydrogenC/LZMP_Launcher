using System;
using System.IO;
using System.Text;
using NBT.Exceptions;
using NBT.IO.Compression.ZLIB;
using System.Collections.Generic;

namespace NBT.IO.Compression
{
    internal static class NBTCompressionHeaders
    {
        private static byte[] GZIP_Header = new byte[2] { 0x1F, 0x8B };

        internal static NBTCompressionType CompressionType(string filePath)
        {
            NBTCompressionType result = NBTCompressionType.Uncompressed;
            using (Stream fStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                result = NBTCompressionHeaders.CompressionType(fStream);
            }
            return result;
        }
        
        internal static NBTCompressionType CompressionType(Stream stream)
        {
            NBTCompressionType result = NBTCompressionType.Uncompressed;
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            if (stream.CanSeek == false)
            {
                throw new NBT_IOException("Can't seek in the stream");
            }
            if (NBTCompressionHeaders.IsGzipStream(stream) == true)
            {
                result = NBTCompressionType.GZipCompression;
            }
            if (NBTCompressionHeaders.IsZlibStream(stream) == true)
            {
                result = NBTCompressionType.ZlibCompression;
            }
            return result;
        }

        internal static bool IsGzipStream(Stream stream)
        {
            bool result = false;
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            if (stream.CanSeek == false)
            {
                throw new NBT_IOException("Can't seek in the stream");
            }
            long initialOffset = stream.Seek(0, SeekOrigin.Current);
            if ((stream.ReadByte() == GZIP_Header[0]) && (stream.ReadByte() == GZIP_Header[1]))
            {
                result = true;
            }
            stream.Seek(initialOffset, SeekOrigin.Begin);
            return result;
        }

        internal static bool IsZlibStream(Stream stream)
        {
            bool result = false;
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            if (stream.CanSeek == false)
            {
                throw new NBT_IOException("Can't seek in the stream");
            }
            long initialOffset = stream.Seek(0, SeekOrigin.Current);

            int CMF = stream.ReadByte();
            int FLG = stream.ReadByte();

            result = ZLibHeader.DecodeHeader(CMF, FLG).IsSupportedZLibStream;

            stream.Seek(initialOffset, SeekOrigin.Begin);
            return result;
        }
    }
}