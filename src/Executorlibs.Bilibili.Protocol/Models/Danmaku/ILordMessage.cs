using Executorlibs.Bilibili.Protocol.Models.Enums;
using Executorlibs.Bilibili.Protocol.Models.General;

namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    /// <summary>
    /// 表示具有老爷类型的信息接口
    /// </summary>
    public interface ILordMessage : IBilibiliMessage
    {
        /// <summary>
        /// 老爷类型
        /// </summary>
        LordType LordType { get; }
    }
}
