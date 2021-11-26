using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Threading.Tasks;
using Executorlibs.Bilibili.Protocol.Clients;
using Executorlibs.Bilibili.Protocol.Invokers.Attributes;
using Executorlibs.Bilibili.Protocol.Models.General;
using Executorlibs.Bilibili.Protocol.Parsers;
using Executorlibs.Bilibili.Protocol.Parsers.Attributes;
using Executorlibs.MessageFramework.Invoking;
using Executorlibs.MessageFramework.Models.General;
using Executorlibs.MessageFramework.Parsers;
using Microsoft.Extensions.Logging;

namespace Executorlibs.Bilibili.Protocol.Invokers
{
    public interface IBilibiliMessageHandlerInvoker : IMessageHandlerInvoker<IDanmakuClient, JsonElement>
    {

    }

    [RegisterBilibiliMessageSubscription(typeof(BilibiliMessageSubscription<>))]
    [RegisterBilibiliMessageSubscriptionResolver(typeof(BilibiliMessageSubscriptionResolver))]
    [RegisterBilibiliMessageParserResolver(typeof(BilibiliMessageParserResolver))]
    public class BilibiliMessageHandlerInvoker : MessageHandlerInvoker<IDanmakuClient, JsonElement>, IBilibiliMessageHandlerInvoker
    {
        public BilibiliMessageHandlerInvoker(IServiceProvider services,
                                             ILogger<BilibiliMessageHandlerInvoker> logger,
                                             IBilibiliMessageSubscriptionResolver subscriptionResolver,
                                             IBilibiliMessageParserResolver parserResolver) : base(services, logger, subscriptionResolver, parserResolver)
        {

        }

#if NETSTANDARD2_0
        protected override bool TryResolveParsers(in JsonElement rawdata, out IEnumerable<IMessageParser<JsonElement>>? parsers)
#else
        protected override bool TryResolveParsers(in JsonElement rawdata, [NotNullWhen(true)] out IEnumerable<IMessageParser<JsonElement>>? parsers)
#endif
        {
            parsers = ParserResolver.ResolveParsers(in rawdata);
            return parsers != null;
        }

#if NETSTANDARD2_0
        protected override bool TryResolveSubscription(Type messageType, out IMessageSubscription? subscription)
#else
        protected override bool TryResolveSubscription(Type messageType, [NotNullWhen(true)]out IMessageSubscription? subscription)
#endif
        {
            return (subscription = SubscriptionResolver.ResolveByMessage(messageType)) != null;
        }

        public override async Task HandleRawdataAsync(IDanmakuClient client, JsonElement rawdata)
        {
            if (TryResolveParsers(in rawdata, out var parsers))
            {
                foreach (var parser in parsers!)
                {
                    if (parser.CanParse(in rawdata))
                    {
                        IMessage<JsonElement> message = parser.Parse(in rawdata);
                        if (TryResolveSubscription(parser.MessageType, out IMessageSubscription? subscription))
                        {
                            if (message is BilibiliMessage bm)
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
