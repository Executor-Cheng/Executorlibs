using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Handlers;
using Executorlibs.MessageFramework.Models.General;

namespace Executorlibs.MessageFramework.Subscriptions
{
    public partial class DefaultMessageSubscription<TClient, TMessage> : MessageSubscription<TClient, TMessage> where TClient : IMessageClient
                                                                                                                where TMessage : IMessage
    {
        protected readonly IMessageHandler<TClient, TMessage>[] _staticHandlers;

        protected Registrations? _registrations;

        public override bool IsEmpty => _staticHandlers.Length == 0 && _registrations?.EffictiveNodeList == null;

        public DefaultMessageSubscription(IEnumerable<IMessageHandler<TClient, TMessage>> handlers) : this(handlers is IMessageHandler<TClient, TMessage>[] array ? array : handlers.ToArray())
        {
            
        }

        protected DefaultMessageSubscription(IMessageHandler<TClient, TMessage>[] staticHandlers)
        {
            _staticHandlers = staticHandlers;
        }

        public override IDisposable AddHandler(IMessageHandler<TClient, TMessage> handler)
        {
            var registrations = Volatile.Read(ref _registrations);
            if (registrations == null)
            {
                registrations = new Registrations();
                registrations = Interlocked.CompareExchange(ref _registrations, registrations, null) ?? registrations;
            }
            var node = registrations.Register(handler);
            return new DynamicHandlerRegistration(node.Id, node);
        }

        public override async Task HandleMessageAsync(TClient client, TMessage message)
        {
            foreach (var handler in this)
            {
                await handler.HandleMessageAsync(client, message);
                if (message.BlockRemainingHandlers)
                {
                    break;
                }
            }
        }

        public override IEnumerator<IMessageHandler<TClient, TMessage>> GetEnumerator()
        {
            return new Enumerator(this);
        }
    }
}
