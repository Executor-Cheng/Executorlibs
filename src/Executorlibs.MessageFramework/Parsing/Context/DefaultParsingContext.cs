using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Dispatchers;
using Executorlibs.MessageFramework.Models.General;
using Executorlibs.MessageFramework.Parsing.Parsers;

namespace Executorlibs.MessageFramework.Parsing.Context
{
    public class DefaultParsingContext<TClient, TRawdata, TMessage> : ParsingContext<TClient, TRawdata> where TClient : IMessageClient
                                                                                                        where TMessage : IMessage<TRawdata>
    {
        protected readonly IMessageParser<TClient, TRawdata, TMessage>[] _parsers;

        protected readonly IMessageDispatcher<TClient, TMessage> _dispatcher;

        public DefaultParsingContext(IEnumerable<IMessageParser<TClient, TRawdata, TMessage>> parsers, IMessageDispatcher<TClient, TMessage> dispatcher) : this(parsers is IMessageParser<TClient, TRawdata, TMessage>[] array ? array : parsers.ToArray(), dispatcher)
        {

        }

        protected DefaultParsingContext(IMessageParser<TClient, TRawdata, TMessage>[] parsers, IMessageDispatcher<TClient, TMessage> dispatcher)
        {
            _parsers = parsers;
            _dispatcher = dispatcher;
        }

        public override bool CanParse(TRawdata rawdata)
        {
            foreach (var parser in _parsers)
            {
                if (parser.CanParse(rawdata))
                {
                    return true;
                }
            }
            return false;
        }

        public override Task InvokeAsync(TClient client, TRawdata rawdata)
        {
            foreach (var parser in _parsers)
            {
                if (parser.CanParse(rawdata))
                {
                    var message = parser.Parse(rawdata);
                    return _dispatcher.HandleMessageAsync(client, message);
                }
            }
            return Task.CompletedTask;
            //if (_parsers.Length != 0)
            //{
            //    int index = 0;
            //    var primaryParser = default(IMessageParser<TClient, TRawdata, TMessage>);
            //    do
            //    {
            //        var parser = _parsers[index];
            //        if (parser.CanParse(rawdata))
            //        {
            //            if (primaryParser != null)
            //            {
            //                return HandleRawdataWithMultipleParserAsync(client, rawdata, primaryParser, parser, index);
            //            }
            //            primaryParser = parser;
            //        }
            //    }
            //    while (++index < _parsers.Length);
            //    if (primaryParser != null)
            //    {
            //        var message = primaryParser.Parse(rawdata);
            //        return _dispatcher.HandleMessageAsync(client, message);
            //    }
            //}
            //return Task.CompletedTask;
        }

        //protected async Task HandleRawdataWithMultipleParserAsync(TClient client, TRawdata rawdata, IMessageParser<TClient, TRawdata, TMessage> primaryParser, IMessageParser<TClient, TRawdata, TMessage> secondaryParser, int index)
        //{
        //    var message = primaryParser.Parse(rawdata);
        //    await _dispatcher.HandleMessageAsync(client, message);
        //    message = secondaryParser.Parse(rawdata);
        //    await _dispatcher.HandleMessageAsync(client, message);
        //    while (++index < _parsers.Length)
        //    {
        //        var parser = _parsers[index];
        //        if (parser.CanParse(rawdata))
        //        {
        //            message = parser.Parse(rawdata);
        //            await _dispatcher.HandleMessageAsync(client, message);
        //        }
        //    }
        //}
    }
}
