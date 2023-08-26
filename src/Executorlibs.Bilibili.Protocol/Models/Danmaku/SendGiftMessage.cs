using System.Diagnostics;
using Executorlibs.Bilibili.Protocol.Parsing.Parsers;
using Executorlibs.Bilibili.Protocol.Parsing.Parsers.Attributes;

namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    /// <summary>
    /// 表示赠送礼物的弹幕信息接口
    /// </summary>
    /// <remarks>
    /// 消息来源是 Bilibili 直播平台<para/>
    /// 继承自 <see cref="ISendGiftBaseMessage"/>
    /// </remarks>
    [RegisterBilibiliParser(typeof(SendGiftParser))]
    public interface ISendGiftMessage : ISendGiftBaseMessage
    {
        /// <summary>
        /// 房间礼物积分
        /// </summary>
        ulong RoomCost { get; }
    }

    /// <summary>
    /// 表示一条赠送礼物的弹幕信息
    /// </summary>
    /// <remarks>
    /// 消息来源是 Bilibili 直播平台
    /// </remarks>
    [DebuggerDisplay("{Time.ToString(\"u\")[..^1],nq} [SendGift] {UserName,nq}[{UserId}]:{GiftName,nq}x{GiftCount}")]
    public class SendGiftMessage : SendGiftBaseMessage, ISendGiftMessage
    {
        /// <inheritdoc/>
        public ulong RoomCost { get; set; }
    }
}
