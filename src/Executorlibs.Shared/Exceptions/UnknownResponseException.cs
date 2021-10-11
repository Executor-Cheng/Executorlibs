using System;
using System.Text.Json;
#if NET6_0_OR_GREATER
using System.Text.Json.Nodes;
#endif

namespace Executorlibs.Shared.Exceptions
{
    public sealed class UnknownResponseException : Exception
    {
        public string? Response { get; }

        public UnknownResponseException() { }

        public UnknownResponseException(in JsonElement root) : this(root.GetRawText()) { }

        public UnknownResponseException(in JsonElement root, string? message) : this(root.GetRawText(), message) { }

        public UnknownResponseException(in JsonElement root, Exception? innerException) : this(root.GetRawText(), innerException) { }

        public UnknownResponseException(in JsonElement root, string? message, Exception? innerException) : this(root.GetRawText(), message, innerException) { }

#if NET6_0_OR_GREATER
        public UnknownResponseException(JsonNode node) : this(node.ToJsonString()) { }

        public UnknownResponseException(JsonNode node, string? message) : this(node.ToJsonString(), message) { }
#endif

        public UnknownResponseException(string response) : this(response, "未知的服务器返回.") { }

        public UnknownResponseException(string response, string? message) : base(message)
            => Response = response;

        public UnknownResponseException(string message, Exception? innerException) : base(message, innerException) { }

        public UnknownResponseException(string response, string? message, Exception? innerException) : base(message, innerException) => Response = response;

        public override string ToString()
        {
            return base.ToString() + "\r\n" + Response;
        }
    }
}
