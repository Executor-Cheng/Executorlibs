using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Handlers;
using Executorlibs.MessageFramework.Models.General;
using Executorlibs.MessageFramework.Subscriptions;

namespace Executorlibs.MessageFramework.Dispatchers
{
    public class DefaultMessageDispatcher<TClient, TMessage> : MessageDispatcher<TClient, TMessage> where TClient : IMessageClient where TMessage : IMessage
    {
        protected readonly MessageSubscription<TClient, TMessage> _subscription;

        public override bool IsEmpty => _subscription.IsEmpty;

        public DefaultMessageDispatcher(IEnumerable<IMessageHandler<TClient, TMessage>> handlers) : this(new DefaultMessageSubscription<TClient, TMessage>(handlers))
        {

        }

        protected DefaultMessageDispatcher(MessageSubscription<TClient, TMessage> subscription)
        {
            _subscription = subscription;
        }

        public override IDisposable AddHandler(IMessageHandler<TClient, TMessage> handler)
        {
            return _subscription.AddHandler(handler);
        }

        public override Task HandleMessageAsync(TClient client, TMessage message)
        {
            return _subscription.HandleMessageAsync(client, message);
        }
    }
}
