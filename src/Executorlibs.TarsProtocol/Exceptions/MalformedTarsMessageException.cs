using System;

namespace Executorlibs.TarsProtocol.Exceptions
{
    public class MalformedTarsMessageException : Exception
    {
        private const string DefaultMessage = "给定的Tar消息不完整。";

        public MalformedTarsMessageException() : this(DefaultMessage)
        {

        }

        public MalformedTarsMessageException(string? message) : this(message, null)
        {

        }

        public MalformedTarsMessageException(string? message, Exception? innerException) : base(message ?? DefaultMessage, innerException)
        {

        }
    }
}
