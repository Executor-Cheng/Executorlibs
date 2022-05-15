using System;
using Executorlibs.TarProtocol.IO;
using Executorlibs.TarProtocol.Models;
using Executorlibs.TarProtocol.Models.Primitives;

namespace Executorlibs.Huya.Protocol.Models
{
    public class WsMsgItem : ITarType
    {
        public long Uri;

        public ArraySegment<byte> Msg;

        public long MsgId;

        public void ReadFrom(ref TarReader reader)
        {
            Uri = reader.Read<TarInt64>();
            Msg = reader.Read<TarByteArray>();
            MsgId = reader.Read<TarInt64>();
        }

        public void WriteTo(ref TarWriter writer)
        {
            new TarInt64(0, Uri).WriteTo(ref writer);
            new TarByteArray(1, Msg).WriteTo(ref writer);
            new TarInt64(2, MsgId).WriteTo(ref writer);
        }
    }
}
