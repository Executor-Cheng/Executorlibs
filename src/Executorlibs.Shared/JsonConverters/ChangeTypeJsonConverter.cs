using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Executorlibs.Shared.JsonConverters
{
    public class ChangeTypeJsonConverter<TFrom, TTo> : JsonConverter<TTo> where TFrom : TTo
    {
        public override TTo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize<TFrom>(ref reader, options)!;
        }

        public override void Write(Utf8JsonWriter writer, TTo value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, (TFrom?)value, options);
        }
    }
}
