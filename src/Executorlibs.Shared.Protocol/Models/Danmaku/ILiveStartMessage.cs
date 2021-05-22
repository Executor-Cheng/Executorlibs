using Executorlibs.Shared.Protocol.Models.General;

namespace Executorlibs.Shared.Protocol.Models.Danmaku
{
    /// <summary>
    /// 全平台通用的直播开始消息接口
    /// </summary>
    public interface ILiveStartMessage : IProtocolMessage
    {

    }

    /// <summary>
    /// 全平台通用的直播开始消息接口
    /// </summary>
    public interface ILiveStartMessage<TRawdata> : ILiveStartMessage, IProtocolMessage<TRawdata>
    {

    }
}
