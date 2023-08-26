using System;
using System.Buffers.Binary;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;
using Executorlibs.Bilibili.Protocol.Utility;

namespace Executorlibs.Bilibili.Protocol.Parsing.Parsers
{
    public interface IPopularityParser : IBilibiliMappableRawMessageParser<IPopularityMessage>
    {

    }

    public class PopularityParser<TMessage> : BilibiliMappableRawMessageParser<IPopularityMessage, TMessage>, IPopularityParser where TMessage : PopularityMessage, new()
    {
        public override uint Key => 3;

        protected override TMessage CreateMessage(byte[] rawdata)
        {
            var message = base.CreateMessage(rawdata);
            var span = DanmakuProtocolUtility.GetBodySpan(rawdata);
            message.Popularity = BinaryPrimitives.ReadUInt32BigEndian(span);
            message.Time = DateTime.Now;
            return message;
        }
    }

    public class PopularityParser : PopularityParser<PopularityMessage>
    {
        public PopularityParser()
        {

        }
    }
}
