using Executorlibs.Shared.Protocol.Models.General;

namespace Executorlibs.Shared.Protocol.Models.Danmaku
{
    /// <summary>
    /// 全平台通用的直播结束消息接口
    /// </summary>
    public interface ILiveEndMessage : IProtocolMessage
    {

    }

    /// <summary>
    /// 全平台通用的直播结束消息接口
    /// </summary>
    public interface ILiveEndMessage<TRawdata> : ILiveEndMessage, IProtocolMessage<TRawdata>
    {

    }
}
