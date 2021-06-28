using System;
using System.IO;
using System.IO.Compression;

using NBT.Tags;
using NBT.Exceptions;
using NBT.IO.Compression;
using NBT.IO.Compression.ZLIB;

namespace NBT.IO
{
    public sealed class NBTFile
    {
        #region "Variables Globales"
            //Variable para almacenar el tipo de compresión que tiene el fichero
            private NBTCompressionType fileType;
            //Almacena la ruta del fichero
            private string filePath;
            //Variable para almacenar el tagCompound principal
            private TagCompound rootTagValue;
            //Variable para almacenar el nombre del tagCompound principal
            private string rootTagName;
        #endregion
        #region "Propiedades"
            public string RootTagName
            {
                get
                {
                    return this.rootTagName;
                }
                set
                {
                    this.rootTagName = value;
                }
            }
            public TagCompound RootTag
            {
                get 
                {
                    return this.rootTagValue;
                }
                set
                {
                    if (value == null)
                    {
                        throw new NBT_InvalidArgumentNullException();
                    }
                    this.rootTagValue = value;
                }
            }
            public string FilePath
            {
                get
                {
                    return this.filePath;
                }
            }
            public NBTCompressionType Compression
            {
                get
                {
                    return this.fileType;
                }
                set
                {
                    this.fileType = value;
                }
            }
        #endregion
        #region "Constructor"
            public NBTFile()
            {
                this.ClearNBTFileInstance();
            }
        #endregion
        #region "Metodos Publicos"
            public void ClearNBTFileInstance()
            {
                this.rootTagName = "";
                this.rootTagValue = new TagCompound();
                this.fileType = NBTCompressionType.Uncompressed;
                this.filePath = "";
            }
            /// <summary>
            /// Load data from stream.
            /// </summary>
            /// <param name="stream">NBT data</param>
            /// <remarks>This function leave open the stream.</remarks>
            public void Load(Stream stream)
            {
                try
                {
                    bool closeAuxStream = false;
                    if (stream == null)
                    {
                        throw new NBT_InvalidArgumentNullException();
                    }
                    NBTCompressionType fileCompression = NBTCompressionHeaders.CompressionType(stream);
                    Stream auxStream = null;
                    switch (fileCompression)
                    {
                        case NBTCompressionType.Uncompressed:
                            {
                                auxStream = stream;
                                closeAuxStream = false;
                                break;
                            }
                        case NBTCompressionType.GZipCompression:
                            {
                                auxStream = new GZipStream(stream, CompressionMode.Decompress, true);
                                closeAuxStream = true;
                                break;
                            }
                        case NBTCompressionType.ZlibCompression:
                            {
                                auxStream = new ZLIBStream(stream, CompressionMode.Decompress, true);
                                closeAuxStream = true;
                                break;
                            }
                    }
                    if (auxStream == null)
                    {
                        throw new NBT_IOException();
                    }
                    byte firstTag = (byte)auxStream.ReadByte();
                    if (firstTag != TagTypes.TagCompound)
                    {
                        throw new NBT_IOException("The first tag must be a TagCompound");
                    }
                    this.fileType = fileCompression;
                    this.rootTagName = TagString.ReadString(auxStream);
                    this.rootTagValue.readTag(auxStream);
                    if (closeAuxStream == true)
                    {
                        auxStream.Close();
                    }
                }
                catch (Exception ex)
                {
                    throw new NBT_IOException("Load exception", ex);
                }
            }
            /// <summary>
            /// Save data into the stream.
            /// </summary>
            /// <param name="stream">Target stream</param>
            /// <remarks>This function leave open the stream.</remarks>
            public void Save(Stream stream)
            {
                try
                {
                    if (stream == null)
                    {
                        throw new NBT_InvalidArgumentNullException();
                    }
                    switch (this.fileType)
                    {
                        case NBTCompressionType.Uncompressed:
                            {
                                Stream rootTagStream = stream;
                                rootTagStream.WriteByte(TagTypes.TagCompound);
                                TagString.WriteString(rootTagStream, this.rootTagName);
                                this.rootTagValue.writeTag(rootTagStream);
                                break;
                            }
                        case NBTCompressionType.GZipCompression:
                            {
                                using (GZipStream gzipStream = new GZipStream(stream, CompressionMode.Compress, true))
                                {
                                    gzipStream.WriteByte(TagTypes.TagCompound);
                                    TagString.WriteString(gzipStream, this.rootTagName);
                                    this.rootTagValue.writeTag(gzipStream);
                                }
                                break;
                            }
                        case NBTCompressionType.ZlibCompression:
                            {
                                using (ZLIBStream zlibStream = new ZLIBStream(stream, CompressionMode.Compress, true))
                                {
                                    zlibStream.WriteByte(TagTypes.TagCompound);
                                    TagString.WriteString(zlibStream, this.rootTagName);
                                    this.rootTagValue.writeTag(zlibStream);
                                }
                                break;
                            }
                    }
                }
                catch (Exception ex)
                {
                    throw new NBT_IOException("Save exception", ex);
                }
            }
            public void Load(string filePath)
            {
                try
                {
                    if (File.Exists(filePath) != true)
                    {
                        throw new NBT_InvalidArgumentException("File not found");
                    }
                    using (Stream stream = File.OpenRead(filePath))
                    {
                        this.Load(stream);
                        this.filePath = filePath;
                    }
                }
                catch (Exception ex)
                {
                    throw new NBT_IOException("Load exception", ex);
                }
            }
            public void Save(string filePath)
            {
                try
                {
                    if (filePath.Trim().Equals("") == true)
                    {
                        throw new NBT_InvalidArgumentException("Filepath can not be empty");
                    }
                    using (Stream fStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        this.Save(fStream);
                        this.filePath = filePath;
                    }
                }
                catch (Exception ex)
                {
                    throw new NBT_IOException("Save exception", ex);
                }
            }
        #endregion
    }
}
