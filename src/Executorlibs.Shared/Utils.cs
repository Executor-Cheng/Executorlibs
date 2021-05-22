using System;
using System.Reflection;
using System.Text.Json;

namespace Executorlibs.Shared
{
    public static class Utils
    {
        /// <summary>
        /// Unix时间戳(秒)
        /// </summary>
        public static int UnixTimeSeconds => (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        /// <summary>
        /// Unix时间戳(毫秒)
        /// </summary>
        public static long UnixTimeMillseconds => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        public static DateTime UnixTime2DateTime(int unixTimeStamp)
            => DateTimeOffset.FromUnixTimeSeconds(unixTimeStamp).DateTime.ToLocalTime();

        public static DateTime UnixTime2DateTime(long unixTimeStamp)
            => DateTimeOffset.FromUnixTimeMilliseconds(unixTimeStamp).DateTime.ToLocalTime();

        public static int DateTime2UnixTimeSeconds(DateTime time)
            => (int)new DateTimeOffset(time).ToUnixTimeSeconds();

        public static long DateTime2UnixTimeMillseconds(DateTime time)
            => new DateTimeOffset(time).ToUnixTimeMilliseconds();

        public static T Deserialize<T>(this in JsonElement element, JsonSerializerOptions? options = null)
        {
            var jsonDocument = JsonDeserialization.JsonDocumentField.GetValue(element);
            ReadOnlyMemory<byte> bytes = (ReadOnlyMemory<byte>)JsonDeserialization.JsonDocumentUtf8JsonField.GetValue(jsonDocument)!;
            return JsonSerializer.Deserialize<T>(bytes.Span, options)!;
        }

        private static class JsonDeserialization
        {
            public static readonly FieldInfo JsonDocumentField = typeof(JsonElement).GetField("_parent", BindingFlags.NonPublic | BindingFlags.Instance)!;
            public static readonly FieldInfo JsonDocumentUtf8JsonField = typeof(JsonDocument).GetField("_utf8Json", BindingFlags.NonPublic | BindingFlags.Instance)!;
        }
    }
}
