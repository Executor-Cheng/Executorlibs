using Executorlibs.Bilibili.Protocol.Models.General;

namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    public interface IOnlineCountMessage : IBilibiliJsonMessage
    {
        uint Count { get; }
    }

    public class OnlineCountMessage : BilibiliJsonMessage, IOnlineCountMessage
    {
        public uint Count { get; set; }
    }
}
