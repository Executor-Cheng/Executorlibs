using System.Collections.Generic;
using Executorlibs.Bilibili.Protocol.Clients;
using Executorlibs.Bilibili.Protocol.Handlers;
using Executorlibs.Bilibili.Protocol.Models.General;
using Executorlibs.MessageFramework.Subscriptions;

namespace Executorlibs.Bilibili.Protocol.Subscriptions
{
    public class BilibiliMessageSubscription<TMessage> : DefaultMessageSubscription<IDanmakuClient, TMessage> where TMessage : IBilibiliMessage
    {
        public BilibiliMessageSubscription(IEnumerable<IBilibiliMessageHandler<TMessage>> handlers) : base(handlers)
        {

        }

        protected BilibiliMessageSubscription(IBilibiliMessageHandler<TMessage>[] staticHandlers) : base(staticHandlers)
        {

        }
    }
}
