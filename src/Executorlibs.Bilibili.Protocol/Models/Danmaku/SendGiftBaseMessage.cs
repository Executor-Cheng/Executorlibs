using System.Text.Json.Serialization;
using Executorlibs.Bilibili.Protocol.Models.Enums;
using Executorlibs.Shared.JsonConverters;
using ISharedSendGiftBaseMessage = Executorlibs.Shared.Protocol.Models.Danmaku.ISendGiftBaseMessage;

namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    /// <summary>
    /// 表示礼物基本信息的接口
    /// </summary>
    /// <remarks>
    /// 消息来源是 Bilibili 直播平台<para/>
    /// 继承自以下接口:
    /// <list type="number">
    /// <item><see cref="ISharedSendGiftBaseMessage"/></item>
    /// <item><see cref="IUserMessage"/></item>
    /// <item><see cref="IGuardMessage"/></item>
    /// </list>
    /// </remarks>
    public interface ISendGiftBaseMessage : ISharedSendGiftBaseMessage, IUserMessage, IGuardMessage
    {
        /// <summary>
        /// 礼物价格 (单位:瓜子)
        /// </summary>
        uint GiftSeedPrice { get; }
        /// <summary>
        /// 是否为金瓜子礼物
        /// </summary>
        bool IsGoldGift { get; }
        /// <summary>
        /// 勋章信息
        /// </summary>
        [JsonConverter(typeof(ChangeTypeJsonConverter<Medal, IMedal>))]
        IMedal? Medal { get; }
    }

    /// <summary>
    /// 实现礼物基本信息接口的抽象类
    /// </summary>
    public abstract class SendGiftBaseMessage : UserMessage, ISendGiftBaseMessage
    {
        /// <inheritdoc/>
        public uint GiftId { get; set; }

        /// <inheritdoc/>
        public string GiftName { get; set; } = null!;

        /// <inheritdoc/>
        public uint GiftCount { get; set; }

        /// <inheritdoc/>
        public uint GiftSeedPrice { get; set; }

        /// <inheritdoc/>
        public bool IsFree { get; set; }

        /// <inheritdoc/>
        public bool IsGoldGift { get => !IsFree; set => IsFree = !value; }

        /// <inheritdoc/>
        public GuardType GuardType { get; set; }

        /// <inheritdoc/>
        [JsonConverter(typeof(ChangeTypeJsonConverter<Medal, IMedal>))]
        public IMedal? Medal { get; set; }

        /// <inheritdoc/>
        public double GiftPrice => GiftSeedPrice / 1000d;
    }
}
