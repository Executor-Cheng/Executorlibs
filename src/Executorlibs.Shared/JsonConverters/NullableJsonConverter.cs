using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Executorlibs.Shared.JsonConverters
{
    public class NullableJsonConverter<T, TConverter> : JsonConverter<T?> where T : struct
                                                                          where TConverter : JsonConverter<T>, new()
    {
        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            TConverter converter = new TConverter();
            return converter.Read(ref reader, typeof(T), options);
        }

        public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options)
        {
            TConverter converter = new TConverter();
            converter.Write(writer, value.GetValueOrDefault(), options);
        }
    }
}
