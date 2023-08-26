using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Handlers;
using Executorlibs.MessageFramework.Models.General;

namespace Executorlibs.MessageFramework.Subscriptions
{
    public interface IMessageSubscription<TClient, TMessage> : IMessageHandler<TClient, TMessage>,
                                                               IEnumerable<IMessageHandler<TClient, TMessage>> where TClient : IMessageClient
                                                                                                               where TMessage : IMessage
    {
        IDisposable AddHandler(IMessageHandler<TClient, TMessage> handler);
    }

    public abstract class MessageSubscription<TClient, TMessage> : IMessageHandler<TClient, TMessage>,
                                                                   IMessageSubscription<TClient, TMessage> where TClient : IMessageClient
                                                                                                           where TMessage : IMessage
    {
        public abstract IDisposable AddHandler(IMessageHandler<TClient, TMessage> handler);

        public abstract Task HandleMessageAsync(TClient client, TMessage message);

        public abstract IEnumerator<IMessageHandler<TClient, TMessage>> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
