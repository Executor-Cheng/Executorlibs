using Executorlibs.MessageFramework.Models.General;

#pragma warning disable CS1712 // Type parameter has no matching typeparam tag in the XML comment (but other type parameters do)
namespace Executorlibs.MessageFramework.Parsing.Parsers
{
    /// <summary>
    /// 表示用于解析原始消息数据到消息实例的接口
    /// </summary>
    /// <typeparam name="TRawdata">原始消息数据类型</typeparam>
    public interface IMessageParser<in TRawdata>
    {
        /// <summary>
        /// 测试给定的 <typeparamref name="TRawdata"/> 能否被处理
        /// </summary>
        /// <remarks>
        /// 请确保此方法不抛出任何异常
        /// </remarks>
        /// <param name="rawdata">消息数据</param>
        bool CanParse(TRawdata rawdata);
    }

    /// <inheritdoc/>
    /// <typeparam name="TMessage">消息实例类型</typeparam>
    public interface IMessageParser<in TRawdata, out TMessage> : IMessageParser<TRawdata> where TMessage : IMessage<TRawdata>
    {
        TMessage Parse(TRawdata rawdata);
    }

    public abstract class MessageParser<TRawdata, TMessage> : IMessageParser<TRawdata, TMessage> where TMessage : IMessage<TRawdata>
    {
        public abstract bool CanParse(TRawdata root);

        public abstract TMessage Parse(TRawdata rawdata);
    }
}
