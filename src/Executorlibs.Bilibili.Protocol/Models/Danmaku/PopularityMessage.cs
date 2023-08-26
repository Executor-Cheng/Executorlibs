using Executorlibs.Bilibili.Protocol.Models.General;
using ISharedPopularityMessage = Executorlibs.Shared.Protocol.Models.Danmaku.IPopularityMessage;

namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    public interface IPopularityMessage : IBilibiliRawMessage, ISharedPopularityMessage
    {

    }

    public class PopularityMessage : BilibiliRawMessage, IPopularityMessage
    {
        public ulong Popularity { get; set; }
    }
}
