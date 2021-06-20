using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Parsers;
using Executorlibs.Bilibili.Protocol.Parsers.Attributes;
using Executorlibs.Shared.Protocol.Models.Danmaku;

namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    /// <summary>
    /// 表示用户关注直播间的接口
    /// </summary>
    /// <remarks>
    /// 消息来源是 Bilibili 直播平台<para/>
    /// 继承自以下接口:
    /// <list type="number">
    /// <item><see cref="IFollowMessage{TRawdata, TUserId}"/></item>
    /// <item><see cref="IInteractMessage"/></item>
    /// </list>
    /// </remarks>
    [RegisterBilibiliParser(typeof(FollowParser))]
    public interface IFollowMessage : IFollowMessage<JsonElement, int>, IInteractMessage
    {

    }

    /// <summary>
    /// 表示一条用户关注房间的消息
    /// </summary>
    public class FollowMessage : InteractMessage, IFollowMessage
    {

    }
}
