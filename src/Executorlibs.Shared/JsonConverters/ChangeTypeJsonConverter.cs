using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Executorlibs.Shared.JsonConverters
{
    public class ChangeTypeJsonConverter<TImpl, TFace> : JsonConverter<TFace> where TImpl : TFace
    {
        [return: MaybeNull]
        public override TFace Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize<TImpl>(ref reader, options);
        }

        public override void Write(Utf8JsonWriter writer, TFace value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, (TImpl?)value, options);
        }
    }
}
