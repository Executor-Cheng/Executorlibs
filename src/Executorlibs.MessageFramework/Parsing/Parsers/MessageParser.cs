using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Models.General;

namespace Executorlibs.MessageFramework.Parsing.Parsers
{
    /// <summary>
    /// 表示用于解析原始消息数据到消息实例的接口
    /// </summary>
    /// <typeparam name="TClient">原始消息数据来源客户端类型</typeparam>
    /// <typeparam name="TRawdata">原始消息数据类型</typeparam>
    public interface IMessageParser<in TClient, TRawdata> where TClient : IMessageClient
    {
        
    }

    public abstract class MessageParser<TClient, TRawdata> : IMessageParser<TClient, TRawdata> where TClient : IMessageClient
    {
        
    }

    /// <summary>
    /// 表示用于解析原始消息数据到消息实例的接口
    /// </summary>
    /// <typeparam name="TClient">原始消息数据来源客户端类型</typeparam>
    /// <typeparam name="TRawdata">原始消息数据类型</typeparam>
    /// <typeparam name="TMessage">消息实例类型</typeparam>
    public interface IMessageParser<in TClient, TRawdata, out TMessage> : IMessageParser<TClient, TRawdata> where TClient : IMessageClient where TMessage : IMessage<TRawdata>
    {
        /// <summary>
        /// 测试给定的 <typeparamref name="TRawdata"/> 能否被处理
        /// </summary>
        /// <remarks>
        /// 请确保此方法不抛出任何异常
        /// </remarks>
        /// <param name="rawdata">消息数据</param>
        bool CanParse(TRawdata rawdata);

        TMessage Parse(TRawdata rawdata);
    }

    public abstract class MessageParser<TClient, TRawdata, TMessage> : MessageParser<TClient, TRawdata>, IMessageParser<TClient, TRawdata, TMessage> where TClient : IMessageClient where TMessage : IMessage<TRawdata>
    {
        public abstract bool CanParse(TRawdata root);

        public abstract TMessage Parse(TRawdata rawdata);
    }
}
