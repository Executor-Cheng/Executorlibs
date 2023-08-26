using Executorlibs.Bilibili.Protocol.Parsing.Parsers;
using Executorlibs.Bilibili.Protocol.Parsing.Parsers.Attributes;
//using ISharedRoomLockMessage = LiveRoomMonitorV3.Shared.Protocol.Models.Danmaku.IRoomLockMessage; // summary broken up

namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    /// <summary>
    /// 表示当前直播间被直播管理员关闭消息的接口
    /// </summary>
    /// <remarks>
    /// 消息来源是 Bilibili 直播平台<para/>
    /// 继承自以下接口:
    /// <list type="number">
    /// <item><see cref="ILiveManagementMessage"/></item>
    /// <item><see cref="Executorlibs.Shared.Protocol.Models.Danmaku.IRoomLockMessage"/></item>
    /// </list>
    /// </remarks>
    [RegisterBilibiliParser(typeof(RoomLockParser))]
    public interface IRoomLockMessage : ILiveManagementMessage, Executorlibs.Shared.Protocol.Models.Danmaku.IRoomLockMessage
    {

    }

    /// <summary>
    /// 表示一条当前直播间被直播管理员关闭消息
    /// </summary>
    /// <remarks>
    /// 消息来源是 Bilibili 直播平台
    /// </remarks>
    public class RoomLockMessage : LiveManagementMessage, IRoomLockMessage
    {

    }
}
