using System;
using System.Diagnostics;
using System.Text.Json.Serialization;
using Executorlibs.Bilibili.Protocol.Parsers;
using Executorlibs.Bilibili.Protocol.Parsers.Attributes;
using Executorlibs.Shared.JsonConverters;

namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    /// <summary>
    /// 表示醒目留言的接口
    /// </summary>
    /// <remarks>
    /// 消息来源是 Bilibili 直播平台<para/>
    /// 继承自以下接口:
    /// <list type="number">
    /// <item><see cref="IDanmakuMessage"/></item>
    /// <item><see cref="ISendGiftBaseMessage"/></item>
    /// </list>
    /// </remarks>
    [RegisterBilibiliParser(typeof(SuperChatParser))]
    public interface ISuperChatMessage : IDanmakuMessage, ISendGiftBaseMessage
    {
        /// <summary>
        /// 悬挂时间。根据 <see cref="ISendGiftBaseMessage.GiftSeedPrice"/> 决定
        /// <list>
        /// <item><term>[30000, 50000)</term><description>60秒</description></item>
        /// <item><term>[50000, 100000)</term><description>2分钟</description></item>
        /// <item><term>[100000, 500000)</term><description>5分钟</description></item>
        /// <item><term>[500000, 1000000)</term><description>30分钟</description></item>
        /// <item><term>[1000000, 2000000)</term><description>1小时</description></item>
        /// <item><term>[2000000, +∞)</term><description>2小时</description></item>
        /// </list>
        /// </summary>
        [JsonConverter(typeof(TimeSpanJsonConverter))]
        TimeSpan KeepTime { get; }
    }

    /// <summary>
    /// 表示一条醒目留言
    /// </summary>
    /// <remarks>
    /// 消息来源是 Bilibili 直播平台
    /// </remarks>
    [DebuggerDisplay("{Time.ToString(\"u\")[..^1],nq} [SuperChat] (${GiftPrice / 1000d}){UserName,nq}[{UserId}]:{Comment,nq}")]
    public class SuperChatMessage : DanmakuMessage, ISuperChatMessage
    {
        /// <inheritdoc/>
        [JsonConverter(typeof(TimeSpanJsonConverter))]
        public TimeSpan KeepTime => GiftSeedPrice switch
        {
            _ when GiftSeedPrice < 50000 => TimeSpan.FromSeconds(60),
            _ when GiftSeedPrice < 100000 => TimeSpan.FromMinutes(2),
            _ when GiftSeedPrice < 500000 => TimeSpan.FromMinutes(5),
            _ when GiftSeedPrice < 1000000 => TimeSpan.FromMinutes(30),
            _ when GiftSeedPrice < 2000000 => TimeSpan.FromHours(1),
            _ => TimeSpan.FromHours(2),
        };

        /// <inheritdoc/>
        public int GiftId => 12000;

        /// <inheritdoc/>
        public string GiftName => "醒目留言";

        /// <inheritdoc/>
        public int GiftCount => 1;

        /// <inheritdoc/>
        public int GiftSeedPrice { get; set; }

        /// <inheritdoc/>
        public bool IsFree => false;

        /// <inheritdoc/>
        public bool IsGoldGift => true;
    }
}
