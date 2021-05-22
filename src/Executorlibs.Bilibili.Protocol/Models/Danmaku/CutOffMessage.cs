using Executorlibs.Bilibili.Protocol.Parsers;
using Executorlibs.Bilibili.Protocol.Parsers.Attributes;
//using ISharedCutOffMessage = LiveRoomMonitorV3.Shared.Protocol.Models.Danmaku.ICutOffMessage; // summary broken up

namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    /// <summary>
    /// 表示当前直播间被直播管理员切断消息的接口
    /// </summary>
    /// <remarks>
    /// 消息来源是 Bilibili 直播平台<para/>
    /// 继承自以下接口:
    /// <list type="number">
    /// <item><see cref="ILiveManagementMessage"/></item>
    /// <item><see cref="Executorlibs.Shared.Protocol.Models.Danmaku.ICutOffMessage"/></item>
    /// </list>
    /// </remarks>
    [RegisterBilibiliParser(typeof(CutOffParser))]
    public interface ICutOffMessage : ILiveManagementMessage, Executorlibs.Shared.Protocol.Models.Danmaku.ICutOffMessage
    {

    }

    /// <summary>
    /// 表示一条直播间被直播管理员切断消息
    /// </summary>
    /// <remarks>
    /// 消息来源是 Bilibili 直播平台
    /// </remarks>
    public class CutOffMessage : LiveManagementMessage, ICutOffMessage
    {

    }
}
