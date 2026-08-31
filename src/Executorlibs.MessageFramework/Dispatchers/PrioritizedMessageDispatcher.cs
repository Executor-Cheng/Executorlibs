using System.Collections.Generic;
using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Handlers;
using Executorlibs.MessageFramework.Models.General;
using Executorlibs.MessageFramework.Subscriptions;

namespace Executorlibs.MessageFramework.Dispatchers
{
    public class PrioritizedMessageDispatcher<TClient, TMessage> : DefaultMessageDispatcher<TClient, TMessage> where TClient : IMessageClient where TMessage : IMessage
    {
        public PrioritizedMessageDispatcher(IEnumerable<IMessageHandler<TClient, TMessage>> handlers) : base(new PrioritizedMessageSubscription<TClient, TMessage>(handlers))
        {

        }
    }
}
