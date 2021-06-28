using System;

namespace NBT.Tags
{
    public static class TagTypes
    {
        #region "Constants"
            //Etiquetas estándar del formato NBT
            public const byte TagEnd = 0;
            public const byte TagByte = 1;
            public const byte TagShort = 2;
            public const byte TagInt = 3;
            public const byte TagLong = 4;
            public const byte TagFloat = 5;
            public const byte TagDouble = 6;
            public const byte TagByteArray = 7;
            public const byte TagString = 8;
            public const byte TagList = 9;
            public const byte TagCompound = 10;
            public const byte TagIntArray = 11;

            //Etiquetas exclusivas no estándar del formato NBT
            public const byte TagImageArray = 237;
            public const byte TagULongArray = 238;
            public const byte TagUIntArray = 239;
            public const byte TagUShortArray = 240;
            public const byte TagSByteArray = 241;
            public const byte TagDoubleArray = 242;
            public const byte TagFloatArray = 243;
            public const byte TagLongArray = 244;
            public const byte TagTimeSpan = 245;
            public const byte TagDateTime = 246;
            public const byte TagShortArray = 247;
            public const byte TagULong = 248;
            public const byte TagUInt = 249;
            public const byte TagUShort = 250;
            public const byte TagSByte = 251;
            public const byte TagImage = 252;
            public const byte TagIP = 253;
            public const byte TagMAC = 254;
        #endregion
    }
}
