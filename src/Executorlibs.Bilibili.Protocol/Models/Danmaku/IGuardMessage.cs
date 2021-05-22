using Executorlibs.Bilibili.Protocol.Models.General;
using Executorlibs.Bilibili.Protocol.Models.Enums;

namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    /// <summary>
    /// 表示具有船员类型的信息接口
    /// </summary>
    public interface IGuardMessage : IBilibiliMessage
    {
        /// <summary>
        /// 船员类型
        /// </summary>
        GuardType GuardType { get; }
    }
}
