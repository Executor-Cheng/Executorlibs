using System.Runtime.CompilerServices;
using Executorlibs.Bilibili.Protocol.Clients;
using Executorlibs.Bilibili.Protocol.Models.General;
using Executorlibs.Bilibili.Protocol.Utility;

namespace Executorlibs.Bilibili.Protocol.Parsing.Parsers
{
    public interface IBilibiliMappableRawMessageParser<out TMessage> : IBilibiliRawMessageParser<TMessage> where TMessage : IBilibiliRawMessage
    {
        /// <summary>
        /// 等同于 <see cref="DanmakuProtocol.Action"/>
        /// </summary>
        uint Key { get; }
    }

    public abstract class BilibiliMappableRawMessageParser<TMessage, TMessageImpl> : BilibiliRawMessageParser<TMessage, TMessageImpl>,
                                                                                     IBilibiliMappableRawMessageParser<TMessage> where TMessage : IBilibiliRawMessage
                                                                                                                                 where TMessageImpl : BilibiliRawMessage, TMessage, new()
    {
        public abstract uint Key { get; }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool CanParse(byte[] rawdata)
        {
            var protocol = DanmakuProtocolUtility.AsProtocol(rawdata);
            return protocol.Action == Key;
        }
    }
}
