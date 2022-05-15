using System;
using Executorlibs.TarProtocol.IO;
using Executorlibs.TarProtocol.Models;
using Executorlibs.TarProtocol.Models.Primitives;

namespace Executorlibs.Huya.Protocol.Models
{
    public class PerSuffixInfo : ITarType // DecorationInfo
    {
        public int AppId;

        public int ViewType;

        public ArraySegment<byte> Data;

        public long SourceId;

        public int Type;

        public void ReadFrom(ref TarReader reader)
        {
            AppId = reader.Read<TarInt32>();
            ViewType = reader.Read<TarInt32>();
            Data = reader.Read<TarByteArray>();
            SourceId = reader.Read<TarInt64>();
            Type = reader.Read<TarInt32>();
        }

        public void WriteTo(ref TarWriter writer)
        {
            throw new NotSupportedException();
        }
    }
}
