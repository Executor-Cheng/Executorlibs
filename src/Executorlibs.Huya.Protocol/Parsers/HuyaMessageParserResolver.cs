using System.Collections.Generic;
using System.Linq;
using Executorlibs.Huya.Protocol.Models.General;
using Executorlibs.MessageFramework.Parsers;

namespace Executorlibs.Huya.Protocol.Parsers
{
    public interface IHuyaMessageParserResolver : IMessageParserResolver<byte[], IHuyaMessageParser>
    {

    }

    public class HuyaMessageParserResolver : IHuyaMessageParserResolver
    {
        //private readonly IReadOnlyDictionary<string, IReadOnlyList<IHuyaMessageParser>> _mappedParsers;

        private readonly IEnumerable<IHuyaMessageParser> _nonmappedParsers;

        public IEnumerable<IHuyaMessageParser> UnknownMessageParsers { get; }

        public HuyaMessageParserResolver(IEnumerable<IHuyaMessageParser> parsers)
        {
            //var mappedParsers = new Dictionary<string, IReadOnlyList<IHuyaMessageParser>>();
            //foreach (IHuyaMessageParser parser in parsers)
            //{
            //    if (parser is IMappableHuyaMessageParser mappableParser && parser.GetType().GetCustomAttribute<SuppressAutoMappingAttribute>() == null)
            //    {
            //        string key = mappableParser.Key;
            //        if (!mappedParsers.TryGetValue(key, out var list))
            //        {
            //            mappedParsers[key] = list = new List<IHuyaMessageParser>();
            //        }
            //        Unsafe.As<IReadOnlyList<IHuyaMessageParser>, List<IHuyaMessageParser>>(ref list).Add(parser);
            //    }
            //}
            _nonmappedParsers = parsers; //.Except(mappedParsers.Values.SelectMany(p => p));
            UnknownMessageParsers = parsers.Where(p => p is IHuyaMessageParser<IUnknownMessage>).ToArray();
            _nonmappedParsers = _nonmappedParsers.Except(UnknownMessageParsers).ToArray();
            //_mappedParsers = mappedParsers;
        }

        public IHuyaMessageParser? ResolveParser(in byte[] rawdata)
        {
            return ResolveParsers(in rawdata).FirstOrDefault();
        }

        public IEnumerable<IHuyaMessageParser> ResolveParsers(in byte[] rawdata)
        {
            //if (rawdata.TryGetProperty("cmd", out byte[] cmdToken) &&
            //    _mappedParsers.TryGetValue(cmdToken.GetString()!, out var mappedParsers))
            //{
            //    return mappedParsers;
            //}
            return _nonmappedParsers;
        }
    }
}
