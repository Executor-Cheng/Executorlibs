using Executorlibs.Bilibili.Protocol.Models.General;
using ISharedPopularityMessage = Executorlibs.Shared.Protocol.Models.Danmaku.IPopularityMessage;

namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    public interface IPopularityMessage : IBilibiliMessage, ISharedPopularityMessage
    {

    }

    public class PopularityMessage : BilibiliMessage, IPopularityMessage
    {
        public ulong Popularity { get; set; }
    }
}
