using System;
using Executorlibs.TarsProtocol.Models;

namespace Executorlibs.TarsProtocol.Exceptions
{
    public class TarsTypeMismatchException : Exception
    {
        private const string DefaultMessage = "给定的Tar消息类型与期望的类型不符。";

        public TarsType? Expected { get; }

        public TarsType? Given { get; }

        public TarsTypeMismatchException() : this(DefaultMessage)
        {

        }

        public TarsTypeMismatchException(string? message) : this(message, null)
        {

        }

        public TarsTypeMismatchException(string? message, Exception? innerException) : base(message ?? DefaultMessage, innerException)
        {

        }

        public TarsTypeMismatchException(TarsType expected, TarsType given) : this(expected, given, null)
        {

        }

        public TarsTypeMismatchException(TarsType expected, TarsType given, string? message) : this(expected, given, message, null)
        {

        }

        public TarsTypeMismatchException(TarsType expected, TarsType given, string? message, Exception? innerException) : this(message, innerException)
        {
            Expected = expected;
            Given = given;
        }
    }
}
