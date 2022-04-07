using System;

namespace Executorlibs.TarProtocol.Exceptions
{
    public class MalformedTarMessageException : Exception
    {
        private const string DefaultMessage = "给定的Tar消息不完整。";

        public MalformedTarMessageException() : this(DefaultMessage)
        {

        }

        public MalformedTarMessageException(string? message) : this(message, null)
        {

        }

        public MalformedTarMessageException(string? message, Exception? innerException) : base(message ?? DefaultMessage, innerException)
        {

        }
    }
}
