using System;
using Executorlibs.TarProtocol.IO;
using Executorlibs.TarProtocol.Models;
using Executorlibs.TarProtocol.Models.Primitives;

namespace Executorlibs.Huya.Protocol.Models
{
    public class NobleLevelInfo : ITarType // NobleLevelInfo
    {
        public int Level;

        public int AttributeType;

        public void ReadFrom(ref TarReader reader)
        {
            Level = reader.Read<TarInt32>();
            AttributeType = reader.Read<TarInt32>();
        }

        public void WriteTo(ref TarWriter writer)
        {
            throw new NotSupportedException();
        }
    }
}
