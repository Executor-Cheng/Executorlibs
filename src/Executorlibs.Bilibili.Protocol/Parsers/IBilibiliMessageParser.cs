using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.General;
using Executorlibs.MessageFramework.Parsers;

namespace Executorlibs.Bilibili.Protocol.Parsers
{
    /// <summary>
    /// 表示处理B站消息的 <see cref="IMessageParser{TRawdata}"/>
    /// </summary>
    public interface IBilibiliMessageParser : IMessageParser<JsonElement>
    {
        
    }

    /// <summary>
    /// 表示处理B站消息的 <see cref="IMessageParser{TRawdata, TMessage}"/>
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IBilibiliMessageParser<TMessage> : IBilibiliMessageParser,
                                                        IMessageParser<JsonElement, TMessage> where TMessage : IBilibiliMessage
    {
        
    }
}
