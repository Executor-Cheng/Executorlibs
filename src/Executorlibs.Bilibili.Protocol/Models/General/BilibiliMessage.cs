using System;
using Executorlibs.MessageFramework.Models.General;
using Executorlibs.Shared.Protocol.Models.General;

namespace Executorlibs.Bilibili.Protocol.Models.General
{
    public interface IBilibiliMessage : IProtocolMessage
    {
        uint RoomId { get; }
    }

    public abstract class BilibiliMessage : Message, IBilibiliMessage
    {
        public ulong Id { get; }

        public uint RoomId { get; set; }

        public DateTime Time { get; }

        public override bool BlockRemainingHandlers { get; set; }
    }
}
