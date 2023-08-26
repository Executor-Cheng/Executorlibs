using Executorlibs.Shared.Protocol.Models.General;

namespace Executorlibs.Shared.Protocol.Models.Danmaku
{
    /// <summary>
    /// 全平台通用的用户基本信息接口
    /// </summary>
    public interface IUserMessage : IProtocolMessage
    {
        /// <summary>
        /// 用户名
        /// </summary>
        string UserName { get; }
    }
}
