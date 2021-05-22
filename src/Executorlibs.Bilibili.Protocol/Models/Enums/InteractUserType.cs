using System;

namespace Executorlibs.Bilibili.Protocol.Models.Enums
{
    /// <summary>
    /// 互动消息用户类型
    /// </summary>
    [Flags]
    public enum InteractUserType
    {
        /// <summary>
        /// 普通用户
        /// </summary>
        Normal = 1 << 0,
        /// <summary>
        /// 房间管理员
        /// </summary>
        Manager = 1 << 1,
        /// <summary>
        /// 粉丝团成员
        /// </summary>
        Fans = 1 << 2,
        /// <summary>
        /// 月费老爷
        /// </summary>
        Vip = 1 << 3,
        /// <summary>
        /// 年费老爷
        /// </summary>
        SVip = 1 << 4,
        /// <summary>
        /// 舰长
        /// </summary>
        Captain = 1 << 5,
        /// <summary>
        /// 提督
        /// </summary>
        Praefect = 1 << 6,
        /// <summary>
        /// 总督
        /// </summary>
        Governor = 1 << 7
    }
}
