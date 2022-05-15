using Executorlibs.Huya.Protocol.Models.General;
using Executorlibs.MessageFramework.Parsers;

namespace Executorlibs.Huya.Protocol.Parsers
{
    /// <summary>
    /// 表示处理B站消息的 <see cref="IMessageParser{TRawdata}"/>
    /// </summary>
    public interface IHuyaMessageParser : IMessageParser<byte[]>
    {

    }

    /// <summary>
    /// 表示处理B站消息的 <see cref="IMessageParser{TRawdata, TMessage}"/>
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IHuyaMessageParser<TMessage> : IHuyaMessageParser,
                                                    IMessageParser<byte[], TMessage> where TMessage : IHuyaMessage
    {

    }

    public abstract class HuyaMessageParser<TMessage> : MessageParser<byte[], TMessage>, IHuyaMessageParser<TMessage> where TMessage : IHuyaMessage
    {

    }
}
