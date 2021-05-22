using Executorlibs.Bilibili.Protocol.Models.General;

namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    /// <summary>
    /// 表示具有房管的信息接口
    /// </summary>
    public interface IAdminMessage : IBilibiliMessage
    {
        /// <summary>
        /// 是否为房管
        /// </summary>
        bool IsAdmin { get; }
    }
}
