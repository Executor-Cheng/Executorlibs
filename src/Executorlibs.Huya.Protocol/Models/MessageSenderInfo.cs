using System;
using Executorlibs.TarProtocol.IO;
using Executorlibs.TarProtocol.Models;
using Executorlibs.TarProtocol.Models.Primitives;

namespace Executorlibs.Huya.Protocol.Models
{
    public class MessageSenderInfo : ITarType // SenderInfo
    {
        public long UserId;

        public long IMid;

        public string UserName = null!;

        public int Gender;

        public string AvartarUrl = null!;

        public int NobleLevel;

        public NobleLevelInfo NobleLevelInfo = null!;

        public string Guid = null!;

        public string UserAgent = null!;

        public void ReadFrom(ref TarReader reader)
        {
            UserId = reader.Read<TarInt64>();
            IMid = reader.Read<TarInt64>();
            UserName = reader.Read<TarString>();
            Gender = reader.Read<TarInt32>();
            AvartarUrl = reader.Read<TarString>();
            NobleLevel = reader.Read<TarInt32>();
            NobleLevelInfo = reader.Read<TarStruct<NobleLevelInfo>>();
            Guid = reader.Read<TarString>();
            UserAgent = reader.Read<TarString>();
        }

        public void WriteTo(ref TarWriter writer)
        {
            throw new NotSupportedException();
        }
    }
}
