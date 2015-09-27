using System;
using System.Runtime.Serialization;

namespace NES
{
    [Serializable]
    internal class NoAssemby : Exception
    {
        public NoAssemby()
        {
        }

        public NoAssemby(string message) : base(message)
        {
        }

        public NoAssemby(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoAssemby(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}