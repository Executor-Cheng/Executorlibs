using System;

namespace Executorlibs.Shared.Exceptions
{
    public sealed class PermissionDeniedException : Exception
    {
        public PermissionDeniedException() : base("无权操作。") { }

        public PermissionDeniedException(string message) : base(message) { }

        public PermissionDeniedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
