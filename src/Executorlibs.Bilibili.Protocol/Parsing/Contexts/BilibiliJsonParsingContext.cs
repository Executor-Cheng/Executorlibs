using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Threading.Tasks;
using Executorlibs.Bilibili.Protocol.Clients;
using Executorlibs.Bilibili.Protocol.Dispatchers;
using Executorlibs.Bilibili.Protocol.Models.General;
using Executorlibs.Bilibili.Protocol.Parsing.Parsers;
using Executorlibs.MessageFramework.Parsing.Context;

namespace Executorlibs.Bilibili.Protocol.Parsing.Contexts
{
    public interface IBilibiliJsonParsingContext : IParsingContext<IDanmakuClient, JsonElement>
    {
#if NETSTANDARD2_0
        bool TryGetMessageKey(out string? key);
#else
        bool TryGetMessageKey([NotNullWhen(true)]out string? key);
#endif
    }

    public interface IBilibiliJsonParsingContext<TMessage> : IBilibiliJsonParsingContext where TMessage : IBilibiliJsonMessage
    {

    }

    public class BilibiliJsonParsingContext<TMessage> : ParsingContext<IDanmakuClient, JsonElement>,
                                                        IBilibiliJsonParsingContext<TMessage> where TMessage : IBilibiliJsonMessage
    {
        protected readonly IBilibiliMessageDispatcher<TMessage> _dispatcher;

        protected readonly IBilibiliMappableJsonMessageParser<TMessage>[] _mappedParsers;

        protected readonly IBilibiliJsonMessageParser<TMessage>[] _nonMappedParsers;

        public BilibiliJsonParsingContext(IBilibiliMessageDispatcher<TMessage> dispatcher, IEnumerable<IBilibiliJsonMessageParser<TMessage>> parsers)
        {
            string? key = null;
            var mappedParsers = new List<IBilibiliMappableJsonMessageParser<TMessage>>(1);
            var nonMappedParsers = new List<IBilibiliJsonMessageParser<TMessage>>();
            foreach (var parser in parsers)
            {
                if (parser is IBilibiliMappableJsonMessageParser<TMessage> mappableParser)
                {
                    if (key == null)
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
#if NETSTANDARD2_0
        public virtual bool TryGetMessageKey(out string? key)
#else
        public virtual bool TryGetMessageKey([NotNullWhen(true)] out string? key)
#endif
        {
            foreach (var mappedParser in _mappedParsers)
            {
                key = mappedParser.Key;
                return true;
            }
            key = null;
            return false;
        }

        public override bool CanParse(JsonElement rawdata)
        {
            if (rawdata.TryGetProperty("cmd", out JsonElement cmdToken) &&
                TryGetMessageKey(out string? key) &&
                cmdToken.GetString() == key)
            {
                foreach (var mappedParser in _mappedParsers)
                {
                    if (mappedParser.CanParse(rawdata))
                    {
                        return true;
                    }
                }
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

        public override Task InvokeAsync(IDanmakuClient client, JsonElement rawdata)
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
