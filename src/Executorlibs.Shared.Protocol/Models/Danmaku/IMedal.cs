namespace Executorlibs.Shared.Protocol.Models.Danmaku
{
    /// <summary>
    /// 全平台通用的勋章基本信息接口
    /// </summary>
    public interface IMedal
    {
        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 等级
        /// </summary>
        int Level { get; }
        /// <summary>
        /// 所属房间号
        /// </summary>
        int RoomId { get; }
        /// <summary>
        /// 所属主播昵称
        /// </summary>
        string Master { get; }
        /// <summary>
        /// 所属主播UID
        /// </summary>
#if NETSTANDARD2_0
        object MasterId { get; }
#else
        object MasterId => throw new System.NotSupportedException();
#endif
        /// <summary>
        /// 颜色
        /// </summary>
        int Color { get; }
    }

    public abstract class Medal : IMedal
    {
        public string Name { get; set; } = null!;

        public int Level { get; set; }

        public int RoomId { get; set; }

        public string Master { get; set; } = null!;

        public int Color { get; set; }

#if NETSTANDARD2_0
        object IMedal.MasterId => throw new System.NotImplementedException();
#endif
    }

    /// <summary>
    /// 全平台通用的勋章基本信息接口
    /// </summary>
    /// <typeparam name="TUserId">用户Id的类型</typeparam>
    public interface IMedal<TUserId> : IMedal
    {
#if !NETSTANDARD2_0
        object IMedal.MasterId => MasterId!;
#endif

        /// <summary>
        /// 所属主播UID
        /// </summary>
        new TUserId MasterId { get; }
    }

    public abstract class Medal<TUserId> : Medal, IMedal<TUserId>
    {
        public TUserId MasterId { get; set; } = default!;

#if NETSTANDARD2_0
        object IMedal.MasterId => MasterId!;
#endif
    }
}
