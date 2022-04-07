using System;
using Executorlibs.TarProtocol.Models;

namespace Executorlibs.TarProtocol.Exceptions
{
    public class TarTypeMismatchException : Exception
    {
        private const string DefaultMessage = "给定的Tar消息类型与期望的类型不符。";

        public TarType? Expected { get; }

        public TarType? Given { get; }

        public TarTypeMismatchException() : this(DefaultMessage)
        {

        }

        public TarTypeMismatchException(string? message) : this(message, null)
        {

        }

        public TarTypeMismatchException(string? message, Exception? innerException) : base(message ?? DefaultMessage, innerException)
        {

        }

        public TarTypeMismatchException(TarType expected, TarType given) : this(expected, given, null)
        {

        }

        public TarTypeMismatchException(TarType expected, TarType given, string? message) : this(expected, given, message, null)
        {

        }

        public TarTypeMismatchException(TarType expected, TarType given, string? message, Exception? innerException) : this(message, innerException)
        {
            Expected = expected;
            Given = given;
        }
    }
}
