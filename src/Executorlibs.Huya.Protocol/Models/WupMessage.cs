using System;
using Executorlibs.TarProtocol.IO;
using Executorlibs.TarProtocol.Models;
using Executorlibs.TarProtocol.Models.Primitives;

namespace Executorlibs.Huya.Protocol.Models
{
    public abstract class WupMessageBase<T> : ITarType // Wup
    {
        public short Version;

        public byte PacketType;

        public int MessageType;

        public int RequestId;

        public string ServantName = null!;

        public string FunctionName = null!;

        public T Data = default!;

        public int Timeout;

        // 还有两个 TarDictionary<TarString, TarString>, 不要了

        public abstract T DeserializeData(ref TarReader reader);

        public abstract void SerializeData(ref TarWriter writer);

        public virtual void ReadFrom(ref TarReader reader)
        {
            Version = reader.Read<TarInt16>();
            PacketType = reader.Read<TarByte>();
            MessageType = reader.Read<TarInt32>();
            RequestId = reader.Read<TarInt32>();
            ServantName = reader.Read<TarString>();
            FunctionName = reader.Read<TarString>();
            Data = DeserializeData(ref reader);
            Timeout = reader.Read<TarInt32>();
        }

        public virtual void WriteTo(ref TarWriter writer)
        {
            ITarType version = new TarInt16(1, Version),
                     packetType = new TarByte(2, PacketType),
                     messageType = new TarInt32(3, MessageType),
                     requestId = new TarInt32(4, RequestId),
                     servantName = new TarString(5, ServantName),
                     functionName = new TarString(6, FunctionName),
                     timeout = new TarInt32(8, Timeout);
            version.WriteTo(ref writer);
            packetType.WriteTo(ref writer);
            messageType.WriteTo(ref writer);
            requestId.WriteTo(ref writer);
            servantName.WriteTo(ref writer);
            functionName.WriteTo(ref writer);
            SerializeData(ref writer);
            timeout.WriteTo(ref writer);
            TarDictionary<TarString, TarString> last2Field = new TarDictionary<TarString, TarString>(9, null);
            last2Field.WriteTo(ref writer);
            last2Field.Header.Tag = 10;
            last2Field.WriteTo(ref writer);
        }
    }

    public sealed class WupMessage : WupMessageBase<ArraySegment<byte>>
    {
        public override ArraySegment<byte> DeserializeData(ref TarReader reader)
        {
            return reader.Read<TarByteArray>();
        }

        public override void SerializeData(ref TarWriter writer)
        {
            TarByteArray data = new TarByteArray(7, Data);
            data.WriteTo(ref writer);
        }
    }

    public sealed class WupMessage<T> : WupMessageBase<T> where T : ITarType, new()
    {
        public override T DeserializeData(ref TarReader reader)
        {
            return reader.Read<TarByteArray<T>>();
        }

        public override void SerializeData(ref TarWriter writer)
        {
            TarByteArray<T> data = new TarByteArray<T>(7, Data);
            data.WriteTo(ref writer);
        }
    }
}
