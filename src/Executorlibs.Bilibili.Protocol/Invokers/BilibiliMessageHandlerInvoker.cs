using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Clients;
using Executorlibs.Bilibili.Protocol.Invokers.Attributes;
using Executorlibs.Bilibili.Protocol.Parsers;
using Executorlibs.Bilibili.Protocol.Parsers.Attributes;
using Executorlibs.MessageFramework.Invoking;
using Executorlibs.MessageFramework.Parsers;
using Microsoft.Extensions.Logging;

namespace Executorlibs.Bilibili.Protocol.Invokers
{
    public interface IBilibiliMessageHandlerInvoker : IMessageHandlerInvoker<IDanmakuClient, JsonElement>
    {

    }

    [RegisterBilibiliMessageSubscription(typeof(BilibiliMessageSubscription<>))]
    [RegisterBilibiliMessageSubscriptionResolver(typeof(BilibiliMessageSubscriptionResolver))]
    [RegisterBilibiliParserResolver(typeof(BilibiliMessageParserResolver))]
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
    }
}
