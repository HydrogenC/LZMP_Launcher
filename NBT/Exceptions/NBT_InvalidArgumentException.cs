using System;

namespace NBT.Exceptions
{
    public sealed class NBT_InvalidArgumentException : Exception
    {
        public NBT_InvalidArgumentException() : base("")
        {
        }
        public NBT_InvalidArgumentException(string message) : base(message) 
        { 
        }
        public NBT_InvalidArgumentException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
