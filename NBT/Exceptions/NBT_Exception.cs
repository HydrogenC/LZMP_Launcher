using System;
namespace NBT.Exceptions
{
    public sealed class NBT_Exception : Exception
    {
        public NBT_Exception() : base("")
        {
        }
        public NBT_Exception(string message) : base(message) 
        { 
        }
        public NBT_Exception(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
