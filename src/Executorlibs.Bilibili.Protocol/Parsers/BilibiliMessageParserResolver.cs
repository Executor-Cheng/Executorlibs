using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.General;
using Executorlibs.MessageFramework.Parsers;
using Executorlibs.MessageFramework.Parsers.Attributes;

namespace Executorlibs.Bilibili.Protocol.Parsers
{
    public interface IBilibiliMessageParserResolver : IMessageParserResolver<JsonElement, IBilibiliMessageParser>
    {

    }

    public class BilibiliMessageParserResolver : IBilibiliMessageParserResolver
    {
        private readonly IReadOnlyDictionary<string, IReadOnlyList<IBilibiliMessageParser>> _mappedParsers;

        private readonly IEnumerable<IBilibiliMessageParser> _nonmappedParsers;

        public IEnumerable<IBilibiliMessageParser> UnknownMessageParsers { get; }

        public BilibiliMessageParserResolver(IEnumerable<IBilibiliMessageParser> parsers)
        {
            var mappedParsers = new Dictionary<string, IReadOnlyList<IBilibiliMessageParser>>();
            foreach (IBilibiliMessageParser parser in parsers)
            {
                if (parser is IMappableBilibiliMessageParser mappableParser && parser.GetType().GetCustomAttribute<SuppressAutoMappingAttribute>() == null)
                {
                    string key = mappableParser.Key;
                    if (!mappedParsers.TryGetValue(key, out var list))
                    {
                        mappedParsers[key] = list = new List<IBilibiliMessageParser>();
                    }
                    Unsafe.As<IReadOnlyList<IBilibiliMessageParser>, List<IBilibiliMessageParser>>(ref list).Add(parser);
                }
            }
            _nonmappedParsers = parsers.Except(mappedParsers.Values.SelectMany(p => p));
            UnknownMessageParsers = parsers.Where(p => p is IBilibiliMessageParser<IUnknownMessage>).ToArray();
            _nonmappedParsers = _nonmappedParsers.Except(UnknownMessageParsers).ToArray();
            _mappedParsers = mappedParsers;
        }

        public IBilibiliMessageParser? ResolveParser(in JsonElement rawdata)
        {
            return ResolveParsers(in rawdata).FirstOrDefault();
        }

        public IEnumerable<IBilibiliMessageParser> ResolveParsers(in JsonElement rawdata)
        {
            if (rawdata.TryGetProperty("cmd", out JsonElement cmdToken) &&
                _mappedParsers.TryGetValue(cmdToken.GetString()!, out var mappedParsers))
            {
                return mappedParsers;
            }
            return _nonmappedParsers.Concat(UnknownMessageParsers);
        }
    }
}
