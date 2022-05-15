using System;
using System.Text.Json;
using System.Threading.Tasks;
using Executorlibs.Bilibili.Protocol.Clients;
using Executorlibs.Bilibili.Protocol.Invokers.Attributes;
using Executorlibs.Bilibili.Protocol.Models.General;
using Executorlibs.Bilibili.Protocol.Parsers;
using Executorlibs.Bilibili.Protocol.Parsers.Attributes;
using Executorlibs.MessageFramework.Invoking;
using Executorlibs.MessageFramework.Models.General;
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
