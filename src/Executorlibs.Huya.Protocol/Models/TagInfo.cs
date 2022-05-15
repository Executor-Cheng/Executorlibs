using System;
using Executorlibs.TarProtocol.IO;
using Executorlibs.TarProtocol.Models;
using Executorlibs.TarProtocol.Models.Primitives;

namespace Executorlibs.Huya.Protocol.Models
{
    public class TagInfo : ITarType
    {
        public int AppId;

        public string Name = null!;

        public void ReadFrom(ref TarReader reader)
        {
            AppId = reader.Read<TarInt32>();
            Name = reader.Read<TarString>();
        }

        public void WriteTo(ref TarWriter writer)
        {
            throw new NotSupportedException();
        }
    }
}
