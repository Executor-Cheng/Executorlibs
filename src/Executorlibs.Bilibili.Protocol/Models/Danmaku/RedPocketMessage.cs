using System;
using Executorlibs.Bilibili.Protocol.Models.General;

namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    public interface IRedPocketMessage : IBilibiliMessage
    {
        int Count { get; }

        string Content { get; }

        string Requirement { get; }

        string Sender { get; }

        int SenderId { get; }

        TimeSpan Duration { get; }
    }

    public class RedPocketMessage : BilibiliMessage, IRedPocketMessage
    {
        public int Count { get; set; }

        public string Content { get; set; } = null!;

        public string Requirement { get; set; } = null!;

        public string Sender { get; set; } = null!;

        public int SenderId { get; set; }

        public TimeSpan Duration { get; set; }
    }
}
