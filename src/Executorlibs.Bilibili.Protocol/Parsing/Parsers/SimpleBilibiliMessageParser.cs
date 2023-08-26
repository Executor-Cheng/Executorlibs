using System;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.General;

namespace Executorlibs.Bilibili.Protocol.Parsing.Parsers
{
    /// <summary>
    /// 处理简单B站消息的 <see cref="IBilibiliJsonMessageParser{TMessage}"/>
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    /// <typeparam name="TMessageImpl"></typeparam>
    public abstract class SimpleBilibiliMessageParser<TMessage, TMessageImpl> : BilibiliMappableJsonMessageParser<TMessage, TMessageImpl> where TMessage : IBilibiliJsonMessage
                                                                                                                                          where TMessageImpl : BilibiliJsonMessage, TMessage, new()
    {
        protected override TMessageImpl CreateMessage(JsonElement rawdata)
        {
            var message = base.CreateMessage(rawdata);
            message.Time = DateTime.Now;
            return message;
        }
    }
}
