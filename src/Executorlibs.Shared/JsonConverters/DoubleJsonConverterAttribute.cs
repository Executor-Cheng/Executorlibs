using System;
using System.Text.Json.Serialization;

namespace Executorlibs.Shared.JsonConverters
{
    public class DoubleJsonConverterAttribute : JsonConverterAttribute
    {
        private readonly int _precision;

        public DoubleJsonConverterAttribute(int precision) : base(null!)
        {
            _precision = precision;
        }

        public override JsonConverter? CreateConverter(Type typeToConvert)
        {
            return new Double2StringJsonConverter(_precision);
        }
    }
}
