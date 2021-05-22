using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.General;
using Executorlibs.Bilibili.Protocol.Parsers;
using Executorlibs.Bilibili.Protocol.Parsers.Attributes;
using Executorlibs.Shared.Protocol.Models.Danmaku;

namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    /// <summary>
    /// 表示主播开播消息的接口
    /// </summary>
    /// <remarks>
    /// 消息来源是 Bilibili 直播平台<para/>
    /// 继承自以下接口:
    /// <list type="number">
    /// <item><see cref="ILiveStartMessage{TRawdata}"/></item>
    /// <item><see cref="IBilibiliMessage"/></item>
    /// </list>
    /// </remarks>
    [RegisterBilibiliParser(typeof(LiveStartParser))]
    public interface ILiveStartMessage : ILiveStartMessage<JsonElement>, IBilibiliMessage
    {

    }

    /// <summary>
    /// 表示一条主播开播消息
    /// </summary>
    /// <remarks>
    /// 消息来源是 Bilibili 直播平台
    /// </remarks>
    public class LiveStartMessage : BilibiliMessage, ILiveStartMessage
    {

    }
}
