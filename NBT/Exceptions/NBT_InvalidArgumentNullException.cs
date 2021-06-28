using System;

namespace NBT.Exceptions
{
    public sealed class NBT_InvalidArgumentNullException : Exception
    {
        public NBT_InvalidArgumentNullException() : base("") 
        { 
        }
        public NBT_InvalidArgumentNullException(string message) : base(message) 
        { 
        }
        public NBT_InvalidArgumentNullException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
