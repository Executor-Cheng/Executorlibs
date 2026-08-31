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
        bool HasNonMappedParser { get; }

        bool TryGetMessageKey([NotNullWhen(true)]out string? key);
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

        public bool HasNonMappedParser => _nonMappedParsers.Length != 0;

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
                        nonMappedParsers.Add(parser);
                        continue;
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

        public virtual bool TryGetMessageKey([NotNullWhen(true)]out string? key)
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
            if (rawdata.TryGetProperty("cmd", out var cmdToken) &&
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
                    message.RoomId = client.RoomId;
                    return _dispatcher.HandleMessageAsync(client, message);
                }
            }
            foreach (var parser in _nonMappedParsers)
            {
                if (parser.CanParse(rawdata))
                {
                    var message = parser.Parse(rawdata);
                    message.RoomId = client.RoomId;
                    return _dispatcher.HandleMessageAsync(client, message);
                }
            }
            return Task.CompletedTask;
        }
    }
}
