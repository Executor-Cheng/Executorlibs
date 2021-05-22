using System;
using System.Text.Json;

namespace Executorlibs.Shared.Exceptions
{
    public sealed class UnknownResponseException : Exception
    {
        public string? Response { get; }

        public UnknownResponseException() { }

        public UnknownResponseException(in JsonElement root) : this(root.GetRawText()) { }

        public UnknownResponseException(in JsonElement root, string message) : this(root.GetRawText(), message) { }

        public UnknownResponseException(string response) : this(response, "未知的服务器返回.") { }

        public UnknownResponseException(string response, string message) : base(message)
            => Response = response;

        public UnknownResponseException(string message, Exception innerException) : base(message, innerException) { }

        public UnknownResponseException(string response, string message, Exception innerException) : base(message, innerException) => Response = response;

        public override string ToString()
        {
            return base.ToString() + "\r\n" + Response;
        }
    }
}
