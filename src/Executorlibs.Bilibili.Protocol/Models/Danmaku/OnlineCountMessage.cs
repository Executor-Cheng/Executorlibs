using Executorlibs.Bilibili.Protocol.Models.General;

namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    public interface IOnlineCountMessage : IBilibiliMessage
    {
        int Count { get; }
    }

    public class OnlineCountMessage : BilibiliMessage, IOnlineCountMessage
    {
        public int Count { get; set; }
    }
}
