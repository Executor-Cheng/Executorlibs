using System;
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
        ulong Id { get; }

        /// <summary>
        /// 消息时间
        /// </summary>
        DateTime Time { get; }
    }

    /// <summary>
    /// 消息的基本信息接口
    /// </summary>
    /// <typeparam name="TRawdata">原始消息数据类型</typeparam>
    public interface IProtocolMessage<TRawdata> : IProtocolMessage, IMessage<TRawdata>
    {

    }
}
