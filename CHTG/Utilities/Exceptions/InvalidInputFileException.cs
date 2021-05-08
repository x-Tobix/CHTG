using System;

namespace CHTG.Exceptions.Utilities
{
    [Serializable]
    public class InvalidInputFileException : Exception
    {
        public InvalidInputFileException() : base() { }
        public InvalidInputFileException(string message) : base(message) { }
        public InvalidInputFileException(string message, Exception inner) : base(message, inner) { }
        protected InvalidInputFileException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
