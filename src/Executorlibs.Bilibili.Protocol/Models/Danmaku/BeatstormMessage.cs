using Executorlibs.Bilibili.Protocol.Models.General;

namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    public interface IBeatstormMessage : IBilibiliJsonMessage
    {
        bool Action { get; }

        uint Count { get; }

        string? Content { get; }
    }

    public class BeatstormMessage : BilibiliJsonMessage, IBeatstormMessage
    {
        public bool Action { get; set; }

        public uint Count { get; set; }

        public string? Content { get; set; }
    }
}
