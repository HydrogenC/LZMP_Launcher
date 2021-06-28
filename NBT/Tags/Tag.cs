using System;
using System.IO;
using System.Net;
using System.Drawing;
using NBT.Exceptions;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace NBT.Tags
{
    public abstract class Tag : ICloneable, IEquatable<Tag>
    {
        public abstract object Value { get; set; }

        public abstract byte tagID { get; }

        public abstract string toString();

        internal abstract void readTag(Stream stream);

        internal abstract void writeTag(Stream stream);

        public abstract object Clone();

        public abstract Type getType();

        public abstract bool Equals(Tag other);

        public virtual Tag this[string key]
        {
            get
            {
                throw new NotSupportedException();
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public virtual Tag this[int index]
        {
            get
            {
                throw new NotSupportedException();
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        internal static Tag ReadTag(Stream stream, byte id)
        {
            switch (id)
            {
                case TagTypes.TagEnd:
                    return new TagEnd();

                case TagTypes.TagByte:
                    return new TagByte(stream);

                case TagTypes.TagShort:
                    return new TagShort(stream);

                case TagTypes.TagInt:
                    return new TagInt(stream);

                case TagTypes.TagLong:
                    return new TagLong(stream);

                case TagTypes.TagFloat:
                    return new TagFloat(stream);

                case TagTypes.TagDouble:
                    return new TagDouble(stream);

                case TagTypes.TagByteArray:
                    return new TagByteArray(stream);

                case TagTypes.TagString:
                    return new TagString(stream);

                case TagTypes.TagList:
                    return new TagList(stream);

                case TagTypes.TagCompound:
                    return new TagCompound(stream);

                case TagTypes.TagIntArray:
                    return new TagIntArray(stream);

                case TagTypes.TagSByte:
                    return new TagSByte(stream);

                case TagTypes.TagUShort:
                    return new TagUShort(stream);

                case TagTypes.TagUInt:
                    return new TagUInt(stream);

                case TagTypes.TagULong:
                    return new TagULong(stream);

                case TagTypes.TagImage:
                    return new TagImage(stream);

                case TagTypes.TagIP:
                    return new TagIP(stream);

                case TagTypes.TagMAC:
                    return new TagMAC(stream);

                case TagTypes.TagShortArray:
                    return new TagShortArray(stream);

                case TagTypes.TagDateTime:
                    return new TagDateTime(stream);

                case TagTypes.TagTimeSpan:
                    return new TagTimeSpan(stream);

                case TagTypes.TagLongArray:
                    return new TagLongArray(stream);

                case TagTypes.TagFloatArray:
                    return new TagFloatArray(stream);

                case TagTypes.TagDoubleArray:
                    return new TagDoubleArray(stream);

                case TagTypes.TagSByteArray:
                    return new TagSByteArray(stream);

                case TagTypes.TagUShortArray:
                    return new TagUShortArray(stream);

                case TagTypes.TagUIntArray:
                    return new TagUIntArray(stream);

                case TagTypes.TagULongArray:
                    return new TagULongArray(stream);

                case TagTypes.TagImageArray:
                    return new TagImageArray(stream);

            }
            throw new NBT_InvalidTagTypeException();
        }

        public static string GetNamedTypeFromId(byte id)
        {
            switch (id)
            {
                case TagTypes.TagEnd:
                    return "TagEnd";

                case TagTypes.TagByte:
                    return "TagByte";

                case TagTypes.TagShort:
                    return "TagShort";

                case TagTypes.TagInt:
                    return "TagInt";

                case TagTypes.TagLong:
                    return "TagLong";

                case TagTypes.TagFloat:
                    return "TagFloat";

                case TagTypes.TagDouble:
                    return "TagDouble";

                case TagTypes.TagByteArray:
                    return "TagByteArray";

                case TagTypes.TagString:
                    return "TagString";

                case TagTypes.TagIntArray:
                    return "TagIntArray";

                case TagTypes.TagList:
                    return "TagList";

                case TagTypes.TagCompound:
                    return "TagCompound";

                case TagTypes.TagSByte:
                    return "TagSByte";

                case TagTypes.TagUShort:
                    return "TagUShort";

                case TagTypes.TagUInt:
                    return "TagUInt";

                case TagTypes.TagULong:
                    return "TagULong";

                case TagTypes.TagImage:
                    return "TagImage";

                case TagTypes.TagIP:
                    return "TagIP";

                case TagTypes.TagMAC:
                    return "TagMAC";

                case TagTypes.TagShortArray:
                    return "TagShortArray";

                case TagTypes.TagDateTime:
                    return "TagDateTime";

                case TagTypes.TagTimeSpan:
                    return "TagTimeSpan";

                case TagTypes.TagLongArray:
                    return "TagLongArray";

                case TagTypes.TagFloatArray:
                    return "TagFloatArray";

                case TagTypes.TagDoubleArray:
                    return "TagDoubleArray";

                case TagTypes.TagSByteArray:
                    return "TagSByteArray";

                case TagTypes.TagUShortArray:
                    return "TagUShortArray";

                case TagTypes.TagUIntArray:
                    return "TagUIntArray";

                case TagTypes.TagULongArray:
                    return "TagULongArray";

                case TagTypes.TagImageArray:
                    return "TagImageArray";

            }
            throw new NBT_InvalidTagTypeException("Unknown TagId '" + id + "'.");
        }

        //Operadores de conversiones
        public static explicit operator byte(Tag value)
        {
            if (value.getType() == typeof(TagByte))
            {
                return ((byte)value.Value);
            }
            throw new NBT_InvalidArgumentException("The parameter must be a TagByte");
        }
        public static explicit operator short(Tag value)
        {
            if (value.getType() == typeof(TagShort))
            {
                return ((short)value.Value);
            }
            throw new NBT_InvalidArgumentException("The parameter must be a TagShort");
        }
        public static explicit operator int(Tag value)
        {
            if (value.getType() == typeof(TagInt))
            {
                return ((int)value.Value);
            }
            throw new NBT_InvalidArgumentException("The parameter must be a TagInt");
        }
        public static explicit operator long(Tag value)
        {
            if (value.getType() == typeof(TagLong))
            {
                return ((long)value.Value);
            }
            throw new NBT_InvalidArgumentException("The parameter must be a TagLong");
        }
        public static explicit operator float(Tag value)
        {
            if (value.getType() == typeof(TagFloat))
            {
                return ((float)value.Value);
            }
            throw new NBT_InvalidArgumentException("The parameter must be a TagFloat");
        }
        public static explicit operator double(Tag value)
        {
            if (value.getType() == typeof(TagDouble))
            {
                return ((double)value.Value);
            }
            throw new NBT_InvalidArgumentException("The parameter must be a TagDouble");
        }
        public static explicit operator byte[](Tag value)
        {
            if (value.getType() == typeof(TagByteArray))
            {
                return ((byte[])value.Value);
            }
            throw new NBT_InvalidArgumentException("The parameter must be a TagByteArray");
        }
        public static explicit operator string(Tag value)
        {
            if (value.getType() == typeof(TagString))
            {
                return ((string)value.Value);
            }
            throw new NBT_InvalidArgumentException("The parameter must be a TagString");
        }
        public static explicit operator List<Tag>(Tag value)
        {
            if (value.getType() == typeof(TagList))
            {
                return ((List<Tag>)value.Value);
            }
            throw new NBT_InvalidArgumentException("The parameter must be a TagList");
        }
        public static explicit operator Dictionary<string, Tag>(Tag value)
        {
            if (value.getType() == typeof(TagCompound))
            {
                return ((Dictionary<string, Tag>)value.Value);
            }
            throw new NBT_InvalidArgumentException("The parameter must be a TagCompound");
        }
        public static explicit operator int[](Tag value)
        {
            if (value.getType() == typeof(TagIntArray))
            {
                return ((int[])value.Value);
            }
            throw new NBT_InvalidArgumentException("The parameter must be a TagIntArray");
        }
        public static explicit operator SByte(Tag value)
        {
            if (value.getType() == typeof(TagSByte)) 
            {
                return ((SByte)value.Value);
            }
            throw new NBT_InvalidArgumentException("The parameter must be a TagSByte");
        }
        public static explicit operator ushort(Tag value)
        {
            if (value.getType() == typeof(TagUShort))
            {
                return ((ushort)value.Value);
            }
            throw new NBT_InvalidArgumentException("The parameter must be a TagUShort");
        }
        public static explicit operator uint(Tag value)
        {
            if (value.getType() == typeof(TagUInt))
            {
                return ((uint)value.Value);
            }
            throw new NBT_InvalidArgumentException("The parameter must be a TagUInt");
        }
        public static explicit operator ulong(Tag value)
        {
            if (value.getType() == typeof(TagULong))
            {
                return ((ulong)value.Value);
            }
            throw new NBT_InvalidArgumentException("The parameter must be a TagULong");
        }
        public static explicit operator Image(Tag value)
        {
            if (value.getType() == typeof(TagImage))
            {
                return ((Image)value.Value);
            }
            throw new NBT_InvalidArgumentException("The parameter must be a TagImage");
        }
        public static explicit operator IPAddress(Tag value)
        {
            if (value.getType() == typeof(TagIP))
            {
                return ((IPAddress)value.Value);
            }
            throw new NBT_InvalidArgumentException("The parameter must be a TagIPv4");
        }
        public static explicit operator PhysicalAddress(Tag value)
        {
            if (value.getType() == typeof(TagMAC))
            {
                return ((PhysicalAddress)value.Value);
            }
            throw new NBT_InvalidArgumentException("The parameter must be a TagMAC");
        }
        public static explicit operator short[](Tag value)
        {
            if (value.getType() == typeof(TagShortArray))
            {
                return ((short[])value.Value);
            }
            throw new NBT_InvalidArgumentException("The parameter must be a TagShortArray");
        }
        public static explicit operator DateTime(Tag value)
        {
            if (value.getType() == typeof(DateTime))
            {
                return ((DateTime)value.Value);
            }
            throw new NBT_InvalidArgumentException("The parameter must be a TagDateTime");
        }
        public static explicit operator TimeSpan(Tag value)
        {
            if (value.getType() == typeof(TimeSpan))
            {
                return ((TimeSpan)value.Value);
            }
            throw new NBT_InvalidArgumentException("The parameter must be a TagTimeSpan");
        }
        public static explicit operator long[](Tag value)
        {
            if (value.getType() == typeof(TagLongArray))
            {
                return ((long[])value.Value);
            }
            throw new NBT_InvalidArgumentException("The parameter must be a TagLongArray");
        }
        public static explicit operator float[](Tag value)
        {
            if (value.getType() == typeof(TagFloatArray)) 
            {
                return ((float[])value.Value);
            }
            throw new NBT_InvalidArgumentException("The parameter must be a TagFloatArray");
        }
        public static explicit operator double[](Tag value) 
        {
            if (value.getType() == typeof(TagDoubleArray))
            {
                return ((double[])value.Value);
            }
            throw new NBT_InvalidArgumentException("The parameter must be a TagDoubleArray");
        }
        public static explicit operator sbyte[](Tag value)
        {
            if (value.getType() == typeof(TagSByteArray))
            {
                return ((sbyte[])value.Value);
            }
            throw new NBT_InvalidArgumentException("The parameter must be a TagSByteArray");
        }
        public static explicit operator ushort[](Tag value)
        {
            if (value.getType() == typeof(TagUShortArray))
            {
                return ((ushort[])value.Value);
            }
            throw new NBT_InvalidArgumentException("The parameter must be a TagUShortArray");
        }
        public static explicit operator uint[](Tag value) 
        {
            if (value.getType() == typeof(TagUIntArray))
            {
                return ((uint[])value.Value);
            }
            throw new NBT_InvalidArgumentException("The parameter must be a TagUIntArray");
        }
        public static explicit operator ulong[](Tag value)
        {
            if (value.getType() == typeof(TagULongArray))
            {
                return ((ulong[])value.Value);
            }
            throw new NBT_InvalidArgumentException("The parameter must be a TagULongArray");
        }
        public static explicit operator Image[](Tag value)
        {
            if (value.getType() == typeof(TagImageArray))
            {
                return ((Image[])value.Value);
            }
            throw new NBT_InvalidArgumentException("The parameter must be a TagImageArray");
        }
    }
}
