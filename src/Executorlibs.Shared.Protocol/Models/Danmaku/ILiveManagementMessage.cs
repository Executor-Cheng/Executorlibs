using Executorlibs.Shared.Protocol.Models.General;

namespace Executorlibs.Shared.Protocol.Models.Danmaku
{
    /// <summary>
    /// 全平台通用的超级管理员进行房间管理操作消息接口
    /// </summary>
    public interface ILiveManagementMessage : IProtocolMessage
    {
        /// <summary>
        /// 操作理由
        /// </summary>
        string? Message { get; }
    }
}
