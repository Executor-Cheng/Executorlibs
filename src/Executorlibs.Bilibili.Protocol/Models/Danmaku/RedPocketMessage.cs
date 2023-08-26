using System;
using Executorlibs.Bilibili.Protocol.Models.General;

namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    public interface IRedPocketMessage : IBilibiliJsonMessage
    {
        uint Count { get; }

        string Content { get; }

        string Requirement { get; }

        string Sender { get; }

        ulong SenderId { get; }

        TimeSpan Duration { get; }
    }

    public class RedPocketMessage : BilibiliJsonMessage, IRedPocketMessage
    {
        public uint Count { get; set; }

        public string Content { get; set; } = null!;

        public string Requirement { get; set; } = null!;

        public string Sender { get; set; } = null!;

        public ulong SenderId { get; set; }

        public TimeSpan Duration { get; set; }
    }
}
