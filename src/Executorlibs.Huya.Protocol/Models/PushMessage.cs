using System;
using Executorlibs.TarProtocol.IO;
using Executorlibs.TarProtocol.Models;
using Executorlibs.TarProtocol.Models.Primitives;

namespace Executorlibs.Huya.Protocol.Models
{
    public class PushMessage : ITarType
    {
        public int PushType; // Tag == 0

        public long Uri;

        public ITarType Msg = null!;

        public int ProtocolType;

        public string GroupId = null!;

        public long MsgId;

        public int MsgTag;

        public void ReadFrom(ref TarReader reader)
        {
            PushType = reader.Read<TarInt32>();
            Uri = reader.Read<TarInt64>();
            Msg = Uri switch
            {
                1400 => reader.Read<TarByteArray<CommentMessage>>(),
                _ => reader.Read<TarByteArray>()
            };
            ProtocolType = reader.Read<TarInt32>();
            GroupId = reader.Read<TarString>();
            MsgId = reader.Read<TarInt64>();
            MsgTag = reader.Read<TarInt32>();
        }

        public void WriteTo(ref TarWriter writer)
        {
            throw new NotSupportedException();
        }

        //public void WriteTo(ref TarWriter writer)
        //{
        //    //new TarInt32(0, PushType).WriteTo(ref writer);
        //    //new TarInt64(1, Uri).WriteTo(ref writer);
        //    //if (Msg is TarByteArray t)
        //    //{
        //    //    t.Header.Tag = 2;
        //    //    t.WriteTo(ref writer);
        //    //}
        //    //else
        //    //{
        //    //    TarByteArray<CommentMessage> msg = (TarByteArray<CommentMessage>)Msg;
        //    //    msg.Header.Tag = 2;
        //    //    msg.WriteTo(ref writer);
        //    //}
        //    //new TarInt32(3, ProtocolType).WriteTo(ref writer);
        //    //new TarString(4, GroupId).WriteTo(ref writer);
        //    //new TarInt64(5, MsgId).WriteTo(ref writer);
        //}
    }
}
