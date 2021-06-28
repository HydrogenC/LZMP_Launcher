/*
 * Alberto Molero 
 * Spain (Catalonia)
 * Last Revision: 16/10/2014
 */

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace NBT.IO.Compression.ZLIB
{
    public enum FLevel
    { 
        Faster = 0,
        Fast = 1,
        Default = 2,
        Optimal = 3,
    }
    public sealed class ZLibHeader
    {
        #region "Variables globales"
            private bool mIsSupportedZLibStream;
            private byte mCompressionMethod; //CMF 0-3
            private byte mCompressionInfo; //CMF 4-7
            private byte mFCheck; //Flag 0-4 (Check bits for CMF and FLG)
            private bool mFDict; //Flag 5 (Preset dictionary)
            private FLevel mFLevel; //Flag 6-7 (Compression level)
        #endregion
        #region "Propiedades"
            public bool IsSupportedZLibStream
            {
                get
                {
                    return this.mIsSupportedZLibStream;
                }
                set
                {
                    this.mIsSupportedZLibStream = value;
                }
            }
            public byte CompressionMethod
            {
                get
                {
                    return this.mCompressionMethod;
                }
                set
                {
                    if (value > 15) 
                    { 
                        throw new ArgumentOutOfRangeException("Argument cannot be greater than 15");
                    }
                    this.mCompressionMethod = value;
                }
            }
            public byte CompressionInfo
            {
                get
                {
                    return this.mCompressionInfo;
                }
                set
                {
                    if (value > 15)
                    {
                        throw new ArgumentOutOfRangeException("Argument cannot be greater than 15");
                    }
                    this.mCompressionInfo = value;
                }
            }
            public byte FCheck 
            {
                get
                {
                    return this.mFCheck;
                }
                set
                {
                    if (value > 31)
                    {
                        throw new ArgumentOutOfRangeException("Argument cannot be greater than 31");
                    }
                    this.mFCheck = value;
                }
            }
            public bool FDict 
            {
                get
                {
                    return this.mFDict;
                }
                set
                {
                    this.mFDict = value;
                }
            }
            public FLevel FLevel 
            {
                get
                {
                    return this.mFLevel;
                }
                set
                {
                    this.mFLevel = value;
                }
            }
        #endregion
        #region "Constructor"
            public ZLibHeader() 
            {

            }
        #endregion
        #region "Metodos privados"
            private void RefreshFCheck()
            {
                string bitsFLG = Convert.ToString(Convert.ToByte(this.FLevel), 2).PadLeft(2, '0') + Convert.ToString(Convert.ToByte(this.FDict), 2);
                byte byteFLG = Convert.ToByte(bitsFLG, 2);
                this.FCheck = Convert.ToByte(31 - Convert.ToByte((this.GetCMF() * 256 + byteFLG) % 31));
            }
            private byte GetCMF()
            {
                string bitsCMF = Convert.ToString(this.CompressionInfo, 2).PadLeft(4, '0') + Convert.ToString(this.CompressionMethod, 2).PadLeft(4, '0');
                return Convert.ToByte(bitsCMF, 2);
            }
            private byte GetFLG()
            {
                string bitsFLG = Convert.ToString(Convert.ToByte(this.FLevel), 2).PadLeft(2, '0') + Convert.ToString(Convert.ToByte(this.FDict), 2) + Convert.ToString(this.FCheck, 2).PadLeft(5, '0');
                return Convert.ToByte(bitsFLG, 2);
            }
        #endregion
        #region "Metodos publicos"
            public byte[] EncodeZlibHeader() 
            {
                byte[] result = new byte[2];

                this.RefreshFCheck();

                result[0] = this.GetCMF();
                result[1] = this.GetFLG();

                return result;
            }
        #endregion
        #region "Metodos estáticos"
            public static ZLibHeader DecodeHeader(int pCMF, int pFlag)
            {
                ZLibHeader result = new ZLibHeader();

                if ((pCMF < byte.MinValue) || (pCMF > byte.MaxValue))
                {
                    throw new ArgumentOutOfRangeException("Argument 'CMF' must be a byte");
                }
                if ((pFlag < byte.MinValue) || (pFlag > byte.MaxValue))
                {
                    throw new ArgumentOutOfRangeException("Argument 'Flag' must be a byte");
                }

                string bitsCMF = Convert.ToString(pCMF, 2).PadLeft(8, '0');
                string bitsFlag = Convert.ToString(pFlag, 2).PadLeft(8, '0');

                result.CompressionInfo = Convert.ToByte(bitsCMF.Substring(0, 4), 2);
                result.CompressionMethod = Convert.ToByte(bitsCMF.Substring(4, 4), 2);

                result.FCheck = Convert.ToByte(bitsFlag.Substring(3, 5), 2);
                result.FDict = Convert.ToBoolean(Convert.ToByte(bitsFlag.Substring(2, 1), 2));
                result.FLevel = (FLevel)Convert.ToByte(bitsFlag.Substring(0, 2), 2);

                result.IsSupportedZLibStream = (result.CompressionMethod == 8) && (result.CompressionInfo == 7) && (((pCMF * 256 + pFlag) % 31 == 0)) && (result.FDict == false);

                return result;
            }
        #endregion
    }
}
