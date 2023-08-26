using Executorlibs.Bilibili.Protocol.Clients;
using Executorlibs.Bilibili.Protocol.Models.General;
using Executorlibs.MessageFramework.Parsing.Parsers;

namespace Executorlibs.Bilibili.Protocol.Parsing.Parsers
{
    public interface IBilibiliRawMessageParser<out TMessage> : IMessageParser<IDanmakuClient, byte[], TMessage> where TMessage : IBilibiliRawMessage
    {

    }

    public abstract class BilibiliRawMessageParser<TMessage, TMessageImpl> : MessageParser<IDanmakuClient, byte[], TMessage>,
                                                                             IBilibiliRawMessageParser<TMessage> where TMessage : IBilibiliRawMessage
                                                                                                                 where TMessageImpl : BilibiliRawMessage, TMessage, new()
    {
        public sealed override TMessage Parse(byte[] rawdata)
        {
            return CreateMessage(rawdata);
        }

        protected virtual TMessageImpl CreateMessage(byte[] rawdata)
        {
            return new TMessageImpl()
            {
                Rawdata = rawdata
            };
        }
    }
}
