using System.Collections.Generic;
using Executorlibs.TarProtocol.IO;

namespace Executorlibs.TarProtocol.Models.Primitives
{
    public struct TarAny : ITarType
    {
        public ITarType[] Objects;

        public void ReadFrom(ref TarReader reader)
        {
            List<ITarType> objects = new List<ITarType>();
            while (reader.TryPeekHeader(out TarHeader header))
            {
                ITarType? instance = header.Type switch
                {
                    TarType.Byte => reader.Read<TarByte>(),
                    TarType.Short => reader.Read<TarInt16>(),
                    TarType.Int => reader.Read<TarInt32>(),
                    TarType.Long => reader.Read<TarInt64>(),
                    TarType.Float => reader.Read<TarFloat>(),
                    TarType.Double => reader.Read<TarDouble>(),
                    TarType.String1 => reader.Read<TarString>(),
                    TarType.String4 => reader.Read<TarString>(),
                    TarType.Map => reader.Read<TarDictionary<TarStruct<TarAny>, TarStruct<TarAny>>>(),
                    TarType.List => reader.Read<TarList<TarStruct<TarAny>>>(),
                    TarType.StructBegin => reader.Read<TarStruct<TarAny>>(),
                    TarType.Zero => reader.Read<TarZero>(),
                    TarType.SimpleList => reader.Read<TarByteArray>(),
                    _ => null
                };
                if (instance == null)
                {
                    break;
                }
                objects.Add(instance);
            }
            Objects = objects.ToArray();
        }

        public void WriteTo(ref TarWriter writer)
        {
        
        }
    }
}
