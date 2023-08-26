using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Executorlibs.Bilibili.Protocol.Clients;
using Executorlibs.Bilibili.Protocol.Dispatchers;
using Executorlibs.Bilibili.Protocol.Models.General;
using Executorlibs.Bilibili.Protocol.Parsing.Parsers;
using Executorlibs.Bilibili.Protocol.Utility;
using Executorlibs.MessageFramework.Parsing.Context;

namespace Executorlibs.Bilibili.Protocol.Parsing.Contexts
{
    public interface IBilibiliRawParsingContext : IParsingContext<IDanmakuClient, byte[]>
    {
        bool TryGetMessageKey(out uint key);
    }

    public abstract class BilibiliRawParsingContext : ParsingContext<IDanmakuClient, byte[]>, IBilibiliRawParsingContext
    {
        public abstract bool TryGetMessageKey(out uint key);
    }

    public interface IBilibiliRawParsingContext<TMessage> : IBilibiliRawParsingContext where TMessage : IBilibiliRawMessage
    {

    }

    public class BilibiliRawParsingContext<TMessage> : BilibiliRawParsingContext,
                                                       IBilibiliRawParsingContext<TMessage> where TMessage : IBilibiliRawMessage
    {
        protected readonly IBilibiliMessageDispatcher<TMessage> _dispatcher;

        protected readonly IBilibiliMappableRawMessageParser<TMessage>[] _mappedParsers;

        protected readonly IBilibiliRawMessageParser<TMessage>[] _nonMappedParsers;

        public BilibiliRawParsingContext(IBilibiliMessageDispatcher<TMessage> dispatcher, IEnumerable<IBilibiliRawMessageParser<TMessage>> parsers)
        {
            uint? key = null;
            var mappedParsers = new List<IBilibiliMappableRawMessageParser<TMessage>>(1);
            var nonMappedParsers = new List<IBilibiliRawMessageParser<TMessage>>();
            foreach (var parser in parsers)
            {
                if (parser is IBilibiliMappableRawMessageParser<TMessage> mappableParser)
                {
                    if (!key.HasValue)
                    {
                        key = mappableParser.Key;
                    }
                    else if (key != mappableParser.Key)
                    {
                        throw new InvalidOperationException("处理同一个消息类型的解析上下文不应具有键值不相同的可映射解析器");
                    }
                    mappedParsers.Add(mappableParser);
                    continue;
                }
                nonMappedParsers.Add(parser);
            }
            _dispatcher = dispatcher;
            _mappedParsers = mappedParsers.ToArray();
            _nonMappedParsers = nonMappedParsers.ToArray();
        }

        public override bool TryGetMessageKey(out uint key)
        {
            foreach (var mappedParser in _mappedParsers)
            {
                key = mappedParser.Key;
                return true;
            }
            key = 0;
            return false;
        }

        public override bool CanParse(byte[] rawdata)
        {
            ref var protocol = ref DanmakuProtocolUtility.AsProtocol(rawdata);
            if (TryGetMessageKey(out uint action) &&
                action == protocol.Action)
            {
                return true;
            }
            foreach (var nonMappedParser in _nonMappedParsers)
            {
                if (nonMappedParser.CanParse(rawdata))
                {
                    return true;
                }
            }
            return false;
        }

        public override Task InvokeAsync(IDanmakuClient client, byte[] rawdata)
        {
            foreach (var parser in _mappedParsers)
            {
                if (parser.CanParse(rawdata))
                {
                    var message = parser.Parse(rawdata);
                    return _dispatcher.HandleMessageAsync(client, message);
                }
            }
            foreach (var parser in _nonMappedParsers)
            {
                if (parser.CanParse(rawdata))
                {
                    var message = parser.Parse(rawdata);
                    return _dispatcher.HandleMessageAsync(client, message);
                }
            }
            return Task.CompletedTask;
        }
    }
}
