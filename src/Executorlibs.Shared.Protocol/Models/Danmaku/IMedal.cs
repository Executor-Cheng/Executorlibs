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
        uint Level { get; }

        /// <summary>
        /// 所属主播昵称
        /// </summary>
        string Master { get; }
    }
}
