using System;
using Executorlibs.TarProtocol.IO;
using Executorlibs.TarProtocol.Models;
using Executorlibs.TarProtocol.Models.Primitives;

namespace Executorlibs.Huya.Protocol.Models
{
    public class WebSocketCommand : ITarType // WebSocketCommand
    {
        public HuyaMsgType CmdType;

        public ArraySegment<byte> Data;

        public long RequestId;

        public string? TraceId;

        public int EncryptType;

        public long Time;

        public virtual void ReadFrom(ref TarReader reader)
        {
            CmdType = (HuyaMsgType)reader.Read<TarByte>().Value;
            Data = reader.Read<TarByteArray>();
            RequestId = reader.Read<TarInt64>();
            TraceId = reader.Read<TarString>();
            EncryptType = reader.Read<TarInt32>();
            Time = reader.Read<TarInt64>();
        }

        public virtual void WriteTo(ref TarWriter writer)
        {
            ITarType cmdType = new TarByte(0, (byte)CmdType),
                     data = new TarByteArray(1, Data),
                     requestId = new TarInt64(2, RequestId),
                     traceId = new TarString(3, TraceId),
                     encryptType = new TarInt32(4, EncryptType),
                     time = new TarInt64(5, Time);
            cmdType.WriteTo(ref writer);
            data.WriteTo(ref writer);
            requestId.WriteTo(ref writer);
            traceId.WriteTo(ref writer);
            encryptType.WriteTo(ref writer);
            time.WriteTo(ref writer);
        }
    }

    public class WebSocketCommand<T> : WebSocketCommand where T : ITarType, new()
    {
        public new T Data = default!;

        public WebSocketCommand() { }

        public WebSocketCommand(HuyaMsgType cmdType, T data)
        {
            CmdType = cmdType;
            Data = data;
        }

        public override void ReadFrom(ref TarReader reader)
        {
            CmdType = (HuyaMsgType)reader.Read<TarByte>().Value;
            Data = reader.Read<TarByteArray<T>>();
            RequestId = reader.Read<TarInt64>();
            TraceId = reader.Read<TarString>();
            EncryptType = reader.Read<TarInt32>();
            Time = reader.Read<TarInt64>();
        }

        public override void WriteTo(ref TarWriter writer)
        {
            new TarByte(0, (byte)CmdType).WriteTo(ref writer);
            new TarByteArray<T>(1, Data).WriteTo(ref writer);
            new TarInt64(2, RequestId).WriteTo(ref writer);
            new TarString(3, TraceId).WriteTo(ref writer);
            new TarInt32(4, EncryptType).WriteTo(ref writer);
            new TarInt64(5, Time).WriteTo(ref writer);
        }
    }
}
