using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Clients;
using Executorlibs.Bilibili.Protocol.Models.General;
using Executorlibs.MessageFramework.Parsing.Parsers;

namespace Executorlibs.Bilibili.Protocol.Parsing.Parsers
{
    /// <summary>
    /// 表示处理B站消息的 <see cref="IMessageParser{IDanmakuClient, JsonElement, TMessage}"/>
    /// </summary>
    /// <typeparam name="TMessage">消息类型</typeparam>
    public interface IBilibiliJsonMessageParser<out TMessage> : IMessageParser<IDanmakuClient, JsonElement, TMessage> where TMessage : IBilibiliJsonMessage
    {

    }

    public abstract class BilibiliJsonMessageParser<TMessage, TMessageImpl> : MessageParser<IDanmakuClient, JsonElement, TMessage>,
                                                                              IBilibiliJsonMessageParser<TMessage> where TMessage : IBilibiliJsonMessage
                                                                                                                   where TMessageImpl : BilibiliJsonMessage, TMessage, new()
    {
        public sealed override TMessage Parse(JsonElement rawdata)
        {
            return CreateMessage(rawdata);
        }

        protected virtual TMessageImpl CreateMessage(JsonElement rawdata)
        {
            return new TMessageImpl()
            {
                Rawdata = rawdata
            };
        }
    }
}
