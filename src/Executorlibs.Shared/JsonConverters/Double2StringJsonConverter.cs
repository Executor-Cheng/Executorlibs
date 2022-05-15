using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Executorlibs.Shared.JsonConverters
{
    public class Double2StringJsonConverter : JsonConverter<double>
    {
        private readonly int _precision;

        public Double2StringJsonConverter()
        {
            _precision = -1;
        }

        public Double2StringJsonConverter(int precision)
        {
            _precision = precision;
        }

        public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return double.Parse(reader.GetString()!);
        }

        public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(_precision switch
            {
                -1 => null,
                0 => "0",
                _ => "0." + new string('0', _precision),
            }));
        }
    }
}
