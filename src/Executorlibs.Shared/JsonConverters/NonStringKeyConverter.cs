using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Executorlibs.Shared.JsonConverters
{
    public sealed class NonStringKeyJsonConverter<TKey, TValue, T> : JsonConverter<T> where T : IDictionary<TKey, TValue>, new()
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            T dictionary = new T();
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.PropertyName:
                        {
                            TKey key = (TKey?)Convert.ChangeType(reader.GetString(), typeof(TKey?))!;
                            reader.Read();
                            dictionary[key] = JsonSerializer.Deserialize<TValue>(ref reader, options!)!;
                            break;
                        }
                    case JsonTokenType.EndObject:
                        {
                            return dictionary;
                        }
                }
            }
            throw new JsonException($"Can't deserialize type {typeof(T).FullName}.");
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            foreach (KeyValuePair<TKey, TValue> pair in value)
            {
                writer.WritePropertyName(pair.Key!.ToString()!);
                JsonSerializer.Serialize(writer, pair.Value, options);
            }
            writer.WriteEndObject();
        }
    }
}
