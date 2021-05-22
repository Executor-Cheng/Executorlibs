using System;

namespace Executorlibs.Shared.Exceptions
{
    public sealed class DuplicateOperationException : Exception
    {
        public DuplicateOperationException() : base("之前已经进行过此操作。") { }

        public DuplicateOperationException(string message) : base(message) { }

        public DuplicateOperationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
