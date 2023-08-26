using Executorlibs.Bilibili.Protocol.Models.General;
using ISharedLiveManagementMessage = Executorlibs.Shared.Protocol.Models.Danmaku.ILiveManagementMessage;

namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    public interface ILiveManagementMessage : IBilibiliJsonMessage, ISharedLiveManagementMessage
    {

    }

    public abstract class LiveManagementMessage : BilibiliJsonMessage, ILiveManagementMessage
    {
        public string? Message { get; set; }
    }
}
