using System.Text.Json.Serialization;
using Executorlibs.Shared.Protocol.Models.Danmaku;

namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    /// <summary>
    /// 表示勋章信息的接口
    /// </summary>
    /// <remarks>
    /// 此类型为 Bilibili 直播平台 专用<para/>
    /// 继承自 <see cref="IMedal{TUserId}"/>
    /// </remarks>
    public interface IMedal : IMedal<int>
    {
        /// <summary>
        /// 前置小图标
        /// </summary>
        int? Badge { get; }
    }

    /// <summary>
    /// 表示勋章信息的类
    /// </summary>
    public class Medal : IMedal
    {
        /// <inheritdoc/>
        public string Name { get; set; } = null!;

        /// <inheritdoc/>
        public int Level { get; set; }

        /// <inheritdoc/>
        public int RoomId { get; set; }

        /// <inheritdoc/>
        public string Master { get; set; } = null!;

        /// <inheritdoc/>
        public int MasterId { get; set; }

        /// <inheritdoc/>
        public int Color { get; set; }

        /// <inheritdoc/>
        public int? Badge { get; set; }
    }
}
