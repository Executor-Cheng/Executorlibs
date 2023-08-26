using Executorlibs.Bilibili.Protocol.Models.General;
using Executorlibs.Bilibili.Protocol.Parsing.Parsers;
using Executorlibs.Bilibili.Protocol.Parsing.Parsers.Attributes;
using ISharedLiveStartMessage = Executorlibs.Shared.Protocol.Models.Danmaku.ILiveStartMessage;

namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    /// <summary>
    /// 表示主播开播消息的接口
    /// </summary>
    /// <remarks>
    /// 消息来源是 Bilibili 直播平台<para/>
    /// 继承自以下接口:
    /// <list type="number">
    /// <item><see cref="ISharedLiveStartMessage"/></item>
    /// <item><see cref="IBilibiliJsonMessage"/></item>
    /// </list>
    /// </remarks>
    [RegisterBilibiliParser(typeof(LiveStartParser))]
    public interface ILiveStartMessage : ISharedLiveStartMessage, IBilibiliJsonMessage
    {

    }

    /// <summary>
    /// 表示一条主播开播消息
    /// </summary>
    /// <remarks>
    /// 消息来源是 Bilibili 直播平台
    /// </remarks>
    public class LiveStartMessage : BilibiliJsonMessage, ILiveStartMessage
    {

    }
}
