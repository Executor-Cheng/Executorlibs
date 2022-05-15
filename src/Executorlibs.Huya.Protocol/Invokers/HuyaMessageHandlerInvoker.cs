using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Executorlibs.Huya.Protocol.Clients;
using Executorlibs.Huya.Protocol.Invokers.Attributes;
using Executorlibs.Huya.Protocol.Models.General;
using Executorlibs.Huya.Protocol.Parsers;
using Executorlibs.Huya.Protocol.Parsers.Attributes;
using Executorlibs.MessageFramework.Invoking;
using Executorlibs.MessageFramework.Models.General;
using Executorlibs.MessageFramework.Parsers;
using Microsoft.Extensions.Logging;

namespace Executorlibs.Huya.Protocol.Invokers
{
    public interface IHuyaMessageHandlerInvoker : IMessageHandlerInvoker<IHuyaClient, byte[]>
    {

    }

    [RegisterHuyaMessageSubscription(typeof(HuyaMessageSubscription<>))]
    [RegisterHuyaMessageSubscriptionResolver(typeof(HuyaMessageSubscriptionResolver))]
    [RegisterHuyaMessageParserResolver(typeof(HuyaMessageParserResolver))]
    public class HuyaMessageHandlerInvoker : MessageHandlerInvoker<IHuyaClient, byte[]>, IHuyaMessageHandlerInvoker
    {
        public HuyaMessageHandlerInvoker(IServiceProvider services,
                                         ILogger<HuyaMessageHandlerInvoker> logger,
                                         IHuyaMessageSubscriptionResolver subscriptionResolver,
                                         IHuyaMessageParserResolver parserResolver) : base(services, logger, subscriptionResolver, parserResolver)
        {

        }

#if NETSTANDARD2_0
        protected override bool TryResolveParsers(in byte[] rawdata, out IEnumerable<IMessageParser<JsonElement>>? parsers)
#else
        protected override bool TryResolveParsers(in byte[] rawdata, [NotNullWhen(true)] out IEnumerable<IMessageParser<byte[]>>? parsers)
#endif
        {
            parsers = ParserResolver.ResolveParsers(in rawdata);
            return parsers != null;
        }

#if NETSTANDARD2_0
        protected override bool TryResolveSubscription(Type messageType, out IMessageSubscription? subscription)
#else
        protected override bool TryResolveSubscription(Type messageType, [NotNullWhen(true)] out IMessageSubscription? subscription)
#endif
        {
            return (subscription = _subscriptionResolver.ResolveByMessage(messageType)) != null;
        }

        public override async Task HandleRawdataAsync(IHuyaClient client, byte[] rawdata)
        {
            if (TryResolveParsers(in rawdata, out var parsers))
            {
                foreach (var parser in parsers!)
                {
                    if (parser.CanParse(in rawdata))
                    {
                        IMessage<byte[]> message = parser.Parse(in rawdata);
                        if (TryResolveSubscription(parser.MessageType, out IMessageSubscription? subscription))
                        {
                            if (message is HuyaMessage bm)
                            {
                                bm.RoomId = client.RoomId;
                            }
                            await subscription!.HandleMessageAsync(client, message).ConfigureAwait(false);
                        }
                    }
                }
            }
        }
    }
}
