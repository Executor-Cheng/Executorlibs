namespace Executorlibs.Shared.Protocol.Models.Danmaku
{
    /// <summary>
    /// 全平台通用的头衔基本信息接口
    /// </summary>
    public interface ITitle
    {
        /// <summary>
        /// 显示名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 序号
        /// </summary>
        uint Id { get; }
    }
}
