using System;
using System.Text.Json.Serialization;
using Executorlibs.MessageFramework.Models.General;

namespace Executorlibs.Shared.Protocol.Models.General
{
    /// <summary>
    /// 消息的基接口
    /// </summary>
    public interface IProtocolMessage : IMessage
    {
        /// <summary>
        /// 消息Id
        /// </summary>
        long Id { get; }

        /// <summary>
        /// 消息时间
        /// </summary>
        DateTime Time { get; }
    }

    /// <summary>
    /// 实现消息的基本信息的抽象类
    /// </summary>
    public abstract class ProtocolMessage : Message, IProtocolMessage
    {

        /// <inheritdoc/>
        public virtual long Id { get; set; }

        /// <inheritdoc/>
        public virtual DateTime Time { get; set; }
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
                                                                                                    // 由于.NET Standard2.0 不支持默认接口实现(CS8701)
                                                                                                    // 这里会有一个继承断层的问题
                                                                                                    // 继承了Message<TRawdata>就不能继承ProtocolMessage
                                                                                                    // 反之亦然, 这会在类上的隐式转换上边出现问题
                                                                                                    // 所以请用接口进行转换
    {

        /// <inheritdoc/>
        public virtual long Id { get; set; }

        /// <inheritdoc/>
        public virtual DateTime Time { get; set; }
    }
}
