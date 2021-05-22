using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Executorlibs.Shared.JsonConverters
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DictKeyAttribute : Attribute
    {

    }

    public class DictValuesConverter<TKey, TValue, T> : JsonConverter<T> where T : IDictionary<TKey, TValue>, new()
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            PropertyInfo? property = typeof(TValue).GetProperties(BindingFlags.Public | BindingFlags.Instance).FirstOrDefault(p => p.IsDefined(typeof(DictKeyAttribute)) && typeof(TKey).IsAssignableFrom(p.PropertyType));
            if (property == null)
            {
                throw new NotSupportedException();
            }
            T dictionary = new T();
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.StartObject:
                        {
                            TValue? value = JsonSerializer.Deserialize<TValue?>(ref reader, options);
                            dictionary[(TKey)property.GetValue(value)!] = value!;
                            break;
                        }
                    case JsonTokenType.EndArray:
                        {
                            return dictionary;
                        }
                }
            }
            throw new JsonException($"Can't deserialize type {typeof(T).FullName}.");
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value.Values, options);
        }
    }
}
