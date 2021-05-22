using System;

namespace Executorlibs.Bilibili.Protocol.Models.Enums
{
    /// <summary>
    /// 互动消息类型
    /// </summary>
    [Flags]
    public enum InteractType
    {
        /// <summary>
        /// 进入直播间
        /// </summary>
        Enter = 1 << 0,
        /// <summary>
        /// 关注直播间
        /// </summary>
        Follow = 1 << 1,
        /// <summary>
        /// 分享直播间
        /// </summary>
        Share = 1 << 2,
        /// <summary>
        /// 特别关注直播间
        /// </summary>
        SpecialFollow = 1 << 3 | Follow,
        /// <summary>
        /// 互相关注
        /// </summary>
        MutualFollow = 1 << 4 | Follow
    }
}
