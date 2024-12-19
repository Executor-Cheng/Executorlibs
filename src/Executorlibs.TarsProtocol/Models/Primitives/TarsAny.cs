using System.Collections.Generic;
using Executorlibs.TarsProtocol.IO;

namespace Executorlibs.TarsProtocol.Models.Primitives
{
    public struct TarsAny : ITarsType
    {
        public ITarsType[] Objects;

        public void ReadFrom(ref TarsReader reader)
        {
            List<ITarsType> objects = new List<ITarsType>();
            while (reader.TryPeekHeader(out TarsHeader header))
            {
                ITarsType? instance = header.Type switch
                {
                    TarsType.Byte => reader.Read<TarsByte>(),
                    TarsType.Short => reader.Read<TarsInt16>(),
                    TarsType.Int => reader.Read<TarsInt32>(),
                    TarsType.Long => reader.Read<TarsInt64>(),
                    TarsType.Float => reader.Read<TarsFloat>(),
                    TarsType.Double => reader.Read<TarsDouble>(),
                    TarsType.String1 => reader.Read<TarsString>(),
                    TarsType.String4 => reader.Read<TarsString>(),
                    TarsType.Map => reader.Read<TarsDictionary<TarsStruct<TarsAny>, TarsStruct<TarsAny>>>(),
                    TarsType.List => reader.Read<TarsList<TarsStruct<TarsAny>>>(),
                    TarsType.StructBegin => reader.Read<TarsStruct<TarsAny>>(),
                    TarsType.Zero => reader.Read<TarsZero>(),
                    TarsType.SimpleList => reader.Read<TarsByteArray>(),
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

        public void WriteTo(ref TarsWriter writer)
        {
            foreach (ITarsType @object in Objects)
            {
                @object.WriteTo(ref writer);
            }
        }
    }
}
