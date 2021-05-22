using Executorlibs.Bilibili.Protocol.Clients;
using Executorlibs.Bilibili.Protocol.Handlers;
using Executorlibs.Bilibili.Protocol.Invokers.Attributes;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;
using Executorlibs.Bilibili.Protocol.Models.General;
using Executorlibs.Bilibili.Protocol.Parsers;
using Executorlibs.MessageFramework.Invoking;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;

namespace Executorlibs.Bilibili.Protocol.Invokers
{
    public interface IBilibiliMessageHandlerInvoker : IMessageHandlerInvoker<IDanmakuClient, JsonElement>
    {

    }

    [RegisterBilibiliMessageSubscription(typeof(BilibiliMessageSubscription<>))]
    public class BilibiliMessageHandlerInvoker : MessageHandlerInvoker<IDanmakuClient, IBilibiliMessageHandler, JsonElement, IBilibiliMessageParser>, IBilibiliMessageHandlerInvoker
    {
        private readonly IReadOnlyDictionary<string, IReadOnlyList<IBilibiliMessageParser>> _MappedParsers;

        //private readonly IEnumerable<IBilibiliMessageParser> _Parsers;

        private readonly IEnumerable<IBilibiliMessageParser> _NonmappedParsers;

        public BilibiliMessageHandlerInvoker(IServiceProvider services, ILogger<BilibiliMessageHandlerInvoker> logger, IEnumerable<IBilibiliMessageParser> parsers)
            : base(services, logger)
        {
            var mappedParsers = new Dictionary<string, IReadOnlyList<IBilibiliMessageParser>>();
            foreach (IBilibiliMessageParser parser in parsers)
            {
                if (parser is IMappableBilibiliMessageParser mappableParser)
                {
                    string key = mappableParser.Key;
                    if (!mappedParsers.TryGetValue(key, out var list))
                    {
                        mappedParsers[key] = list = new List<IBilibiliMessageParser>();
                    }
                    Unsafe.As<IReadOnlyList<IBilibiliMessageParser>, List<IBilibiliMessageParser>>(ref list).Add(parser);
                }
            }
            //_Parsers = parsers;
            _NonmappedParsers = parsers.Except(mappedParsers.Values.SelectMany(p => p)).ToArray();
            _MappedParsers = mappedParsers;
        }

        public override async Task HandleRawdataAsync(IDanmakuClient client, JsonElement rawdata)
        {
            if (TryResolveParsers(in rawdata, out var parsers))
            {
                foreach (var parser in parsers)
                {
                    if (parser.CanParse(in rawdata))
                    {
                        IBilibiliMessage message = (IBilibiliMessage)parser.Parse(in rawdata);
                        if (message is BilibiliMessage bm)
                        {
                            bm.RoomId = client.RoomId;
                        }
                        if (TryResolveSubscription(parser.MessageType, out IBilibiliMessageSubscription? subscription))
                        {
                            await subscription.HandleMessage(client, message);
                        }
                    }
                }
            }
        }

        protected override bool TryResolveParsers(in JsonElement rawdata, [NotNullWhen(true)]out IEnumerable<IBilibiliMessageParser>? parsers)
        {
            if (rawdata.TryGetProperty("cmd", out JsonElement cmdToken) &&
                _MappedParsers.TryGetValue(cmdToken.GetString()!, out var tparsers))
            {
                parsers = tparsers;
            }
            else
            {
                parsers = _NonmappedParsers;
            }
            return true;
        }

        protected virtual bool TryResolveSubscription<TMessage>([NotNullWhen(true)]out IBilibiliMessageSubscription<TMessage>? subscription) where TMessage : IBilibiliMessage
        {
            Unsafe.SkipInit(out subscription);
            ref IBilibiliMessageSubscription? s = ref Unsafe.As<IBilibiliMessageSubscription<TMessage>?, IBilibiliMessageSubscription?>(ref subscription);
            return TryResolveSubscription(typeof(TMessage), out s);
        }

        protected virtual bool TryResolveSubscription(Type messageType, [NotNullWhen(true)]out IBilibiliMessageSubscription? subscription)
        {
            if (!_SubscriptionMapping.TryGetValue(messageType, out Type? subscriptionType))
            {
                _SubscriptionMapping[messageType] = subscriptionType = typeof(IBilibiliMessageSubscription<>).MakeGenericType(messageType);
            }
            Unsafe.SkipInit(out subscription);
            ref object? t = ref Unsafe.As<IBilibiliMessageSubscription?, object?>(ref subscription);
            return (t = _Services.GetService(subscriptionType)) != null;
        }

        protected override bool TryResolveSubscription(Type messageType, [NotNullWhen(true)]out IMessageSubscription? subscription)
        {
            Unsafe.SkipInit(out subscription);
            ref IBilibiliMessageSubscription? s = ref Unsafe.As<IMessageSubscription?, IBilibiliMessageSubscription?>(ref subscription);
            return TryResolveSubscription(messageType, out s);
        }
    }
}
