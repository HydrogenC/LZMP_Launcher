using System;

namespace NBT.Exceptions
{
    public sealed class NBT_IOException : Exception 
    {
        public NBT_IOException() : base("") 
        { 
        }

        public NBT_IOException(string message) : base(message) 
        { 
        }

        public NBT_IOException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
