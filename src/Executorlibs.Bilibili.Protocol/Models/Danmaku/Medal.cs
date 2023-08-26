using ISharedMedal = Executorlibs.Shared.Protocol.Models.Danmaku.IMedal;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    /// <summary>
    /// 表示勋章信息的接口
    /// </summary>
    /// <remarks>
    /// 此类型为 Bilibili 直播平台 专用<para/>
    /// 继承自 <see cref="ISharedMedal"/>
    /// </remarks>
    public interface IMedal : ISharedMedal
    {
        /// <summary>
        /// 主播房间号
        /// </summary>
        uint RoomId { get; }

        /// <summary>
        /// 主播用户Id
        /// </summary>
        ulong MasterId { get; }

        /// <summary>
        /// 前置小图标Id
        /// </summary>
        uint? Badge { get; }

        /// <summary>
        /// rgb颜色
        /// </summary>
        uint Color { get; }
    }

    /// <summary>
    /// 表示勋章信息的类
    /// </summary>
    public class Medal : IMedal
    {
        /// <inheritdoc/>
        public string Name { get; set; }

        /// <inheritdoc/>
        public uint RoomId { get; set; }

        /// <inheritdoc/>
        public uint Level { get; set; }

        /// <inheritdoc/>
        public string Master { get; set; }

        /// <inheritdoc/>
        public ulong MasterId { get; set; }

        /// <inheritdoc/>
        public uint? Badge { get; set; }

        /// <inheritdoc/>
        public uint Color { get; set; }
    }
}
