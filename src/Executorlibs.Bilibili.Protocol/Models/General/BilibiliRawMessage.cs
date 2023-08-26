using System;
using Executorlibs.MessageFramework.Models.General;
using Executorlibs.Shared.Protocol.Models.General;

namespace Executorlibs.Bilibili.Protocol.Models.General
{
    public interface IBilibiliRawMessage : IBilibiliMessage, IProtocolMessage<byte[]>
    {
        
    }

    public abstract class BilibiliRawMessage : Message<byte[]>, IBilibiliRawMessage
    {
        public ulong Id { get; set; }

        public uint RoomId { get; set; }

        public DateTime Time { get; set; }

        protected override byte[]? DeepClone()
        {
            if (Rawdata is byte[] rawdata)
            {
                if (rawdata.Length == 0)
                {
                    return rawdata;
                }
                return (byte[])rawdata.Clone();
            }
            return null;
        }
    }
}
