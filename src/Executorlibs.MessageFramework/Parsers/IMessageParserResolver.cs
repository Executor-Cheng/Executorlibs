using System.Collections.Generic;

namespace Executorlibs.MessageFramework.Parsers
{
    public interface IMessageParserResolver<TRawdata, out TParserService> where TParserService : IMessageParser
    {
        TParserService? ResolveParser(in TRawdata rawdata);

        IEnumerable<TParserService> ResolveParsers(in TRawdata rawdata);
    }
}
