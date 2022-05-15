using Executorlibs.MessageFramework.Models.General;
using Executorlibs.Shared.Protocol.Models.General;

namespace Executorlibs.Huya.Protocol.Models.General
{
    /// <summary>
    /// 表示由B站发出的消息
    /// </summary>
    /// <remarks>
    /// 继承自 <see cref="IMessage{JsonElement}"/>
    /// </remarks>
    public interface IHuyaMessage : IProtocolMessage<byte[]>
    {
        /// <summary>
        /// 房间号
        /// </summary>
        long RoomId { get; }
    }

    /// <summary>
    /// 实现 <see cref="IHuyaMessage"/> 的抽象类
    /// </summary>
    public abstract class HuyaMessage : ProtocolMessage<byte[]>, IHuyaMessage
    {
        /// <inheritdoc/>
        public long RoomId { get; set; }
    }
}
