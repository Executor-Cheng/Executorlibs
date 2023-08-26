using System;
using Executorlibs.Bilibili.Protocol.Models.General;

namespace Executorlibs.Bilibili.Protocol.Parsing.Parsers
{

    public interface IUnknownRawMessageParser : IBilibiliRawMessageParser<IUnknownRawMessage>
    {

    }

    public class UnknownRawMessageParser<TMessage> : BilibiliRawMessageParser<IUnknownRawMessage, TMessage>, IUnknownRawMessageParser where TMessage : UnknownRawMessage, new()
    {
        public override bool CanParse(byte[] root)
        {
            return true;
        }

        protected override TMessage CreateMessage(byte[] rawdata)
        {
            var message = base.CreateMessage(rawdata);
            message.Time = DateTime.Now;
            return message;
        }
    }

    public sealed class UnknownRawMessageParser : UnknownRawMessageParser<UnknownRawMessage>
    {
        public UnknownRawMessageParser()
        {

        }
    }
}
