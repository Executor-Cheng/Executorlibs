using Executorlibs.Bilibili.Protocol.Models.General;

namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    public interface IBeatstormMessage : IBilibiliMessage
    {
        bool Action { get; }

        int Count { get; }

        string? Content { get; }
    }

    public class BeatstormMessage : BilibiliMessage, IBeatstormMessage
    {
        public bool Action { get; set; }

        public int Count { get; set; }

        public string? Content { get; set; }
    }
}
