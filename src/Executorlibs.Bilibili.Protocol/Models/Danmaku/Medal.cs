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
    public interface IMedal : IMedal<long>
    {
        /// <summary>
        /// 前置小图标
        /// </summary>
        int? Badge { get; }
    }

    /// <summary>
    /// 表示勋章信息的类
    /// </summary>
    public class Medal : Medal<long>, IMedal
    {
        /// <inheritdoc/>
        public int? Badge { get; set; }
    }
}
