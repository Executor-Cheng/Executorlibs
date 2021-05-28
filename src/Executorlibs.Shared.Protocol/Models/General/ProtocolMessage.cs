using Executorlibs.MessageFramework.Models.General;

namespace Executorlibs.Shared.Protocol.Models.General
{
    /// <summary>
    /// 消息的基接口
    /// </summary>
    public interface IProtocolMessage : IMessage
    {
        /// <summary>
        /// 房间号
        /// </summary>
        int RoomId { get; }
    }

    /// <summary>
    /// 消息的基本信息接口
    /// </summary>
    /// <typeparam name="TRawdata">原始消息数据类型</typeparam>
    /// <remarks>
    /// 继承 <see cref="IProtocolMessage"/>
    /// </remarks>
    public interface IProtocolMessage<TRawdata> : IProtocolMessage, IMessage<TRawdata>
    {

    }

    /// <summary>
    /// 实现消息的基本信息的抽象类
    /// </summary>
    /// <typeparam name="TRawdata">原始消息数据类型</typeparam>
    public abstract class ProtocolMessage<TRawdata> : Message<TRawdata>, IProtocolMessage<TRawdata> // 建议传弹幕消息只使用接口（方便继承）
    {
        /// <inheritdoc/>
        public int RoomId { get; set; }
    }
}
