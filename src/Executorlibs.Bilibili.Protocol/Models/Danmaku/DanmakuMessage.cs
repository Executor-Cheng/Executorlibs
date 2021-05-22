using System.Diagnostics;
using System.Text.Json.Serialization;
using Executorlibs.Bilibili.Protocol.Parsers;
using Executorlibs.Bilibili.Protocol.Parsers.Attributes;
using Executorlibs.Bilibili.Protocol.Models.Enums;
using Executorlibs.Shared.JsonConverters;

namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    /// <summary>
    /// 表示普通弹幕的接口
    /// </summary>
    /// <remarks>
    /// 消息来源是 Bilibili 直播平台<para/>
    /// 继承自以下接口:
    /// <list type="number">
    /// <item><see cref="IDanmakuBaseMessage"/></item>
    /// <item><see cref="IGuardMessage"/></item>
    /// <item><see cref="ILordMessage"/></item>
    /// <item><see cref="IAdminMessage"/></item>
    /// </list>
    /// </remarks>
    [RegisterBilibiliParser(typeof(DanmakuParser))]
    public interface IDanmakuMessage : IDanmakuBaseMessage, IGuardMessage, ILordMessage, IAdminMessage
    {
        /// <summary>
        /// 弹幕类型
        /// </summary>
        DanmakuMode Mode { get; }
        /// <summary>
        /// 用户排名
        /// </summary>
        int? Rank { get; }
        /// <summary>
        /// 是否为抽奖弹幕
        /// </summary>
        bool IsLotteryDanmaku { get; } // 0.9 > 0
        /// <summary>
        /// 头衔信息
        /// </summary>
        [JsonConverter(typeof(ChangeTypeJsonConverter<Title, ITitle>))]
        ITitle? Title { get; }
        /// <summary>
        /// 勋章信息
        /// </summary>
        [JsonConverter(typeof(ChangeTypeJsonConverter<Medal, IMedal>))]
        IMedal? Medal { get; }
    }

    /// <summary>
    /// 表示一条弹幕消息
    /// </summary>
    /// <remarks>
    /// 消息来源是 Bilibili 直播平台
    /// </remarks>
    [DebuggerDisplay("{Time.ToString(\"u\")[..^1],nq} [Danmaku] {UserName,nq}[{UserId}]:{Comment,nq}")]
    public class DanmakuMessage : DanmakuBaseMessage, IDanmakuMessage
    {
        /// <inheritdoc/>
        public DanmakuMode Mode { get; set; }

        /// <inheritdoc/>
        public int? Rank { get; set; }

        /// <inheritdoc/>
        public bool IsAdmin { get; set; }

        /// <inheritdoc/>
        public bool IsLotteryDanmaku { get; set; }

        /// <inheritdoc/>
        [JsonConverter(typeof(ChangeTypeJsonConverter<Title, ITitle>))]
        public ITitle? Title { get; set; }

        /// <inheritdoc/>
        [JsonConverter(typeof(ChangeTypeJsonConverter<Medal, IMedal>))]
        public IMedal? Medal { get; set; }

        /// <inheritdoc/>
        public GuardType GuardType { get; set; }

        /// <inheritdoc/>
        public LordType LordType { get; set; }
    }
}
