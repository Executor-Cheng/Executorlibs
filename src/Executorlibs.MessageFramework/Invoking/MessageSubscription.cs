using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Handlers;
using Executorlibs.MessageFramework.Models.General;

namespace Executorlibs.MessageFramework.Invoking
{
    public abstract partial class MessageSubscription : IMessageSubscription
    {
        protected IMessageHandler[] StaticHandlers { get; }

        protected Registrations? _registrations;

        protected bool _disposed;

        protected MessageSubscription(IEnumerable<IMessageHandler> handlers)
        {
            if (handlers.Any())
            {
                StaticHandlers = ResolveStaticHandlers(new LinkedList<IMessageHandler>(handlers), new List<IMessageHandler>());
            }
            else
            {
                StaticHandlers = Array.Empty<IMessageHandler>();
            }
        }

        protected abstract IMessageHandler[] ResolveStaticHandlers(LinkedList<IMessageHandler> handlers, List<IMessageHandler> filtered);

        public abstract DynamicHandlerRegistration AddHandler(IMessageHandler handler);

        public virtual Task HandleMessageAsync(IMessageClient client, IMessage message)
        {
            throw new NotSupportedException("请使用泛型接口中的 HandleMessageAsync 方法。");
        }

        public virtual IEnumerator<IMessageHandler> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected virtual void Dispose(bool disposing)
        {
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    public partial class MessageSubscription<TClient, TMessage> : MessageSubscription, IMessageSubscription<TClient, TMessage> where TClient : IMessageClient
                                                                                                                               where TMessage : IMessage
    {
        public MessageSubscription(IEnumerable<IMessageHandler> handlers) : base(handlers)
        {

        }

        protected override IMessageHandler[] ResolveStaticHandlers(LinkedList<IMessageHandler> handlers, List<IMessageHandler> filtered)
        {
            if (handlers.Count != 0)
            {
                for (LinkedListNode<IMessageHandler>? handlerNode = handlers.First; handlerNode != null; handlerNode = handlerNode.Next)
                {
                    IMessageHandler handler = handlerNode.Value;
                    if (handler is IContravarianceMessageHandler<TClient, TMessage> ||
                        handler is IInvarianceMessageHandler<TClient, TMessage>)
                    {
                        filtered.Add(handler);
                        handlers.Remove(handlerNode);
                    }
                }
            }
            return filtered.ToArray();
        }

        public override DynamicHandlerRegistration AddHandler(IMessageHandler handler)
        {
            return AddHandler((IMessageHandler<TClient, TMessage>)handler);
        }

        public virtual DynamicHandlerRegistration AddHandler(IMessageHandler<TClient, TMessage> handler)
        {
            Registrations? registrations = Volatile.Read(ref _registrations);
            if (registrations is null)
            {
                registrations = new Registrations(this);
                registrations = Interlocked.CompareExchange(ref _registrations, registrations, null) ?? registrations;
            }

            RegistrationNode? node = registrations.Register(handler);
            long id = node.Id;

            if (!_disposed || !registrations.Unregister(id, node))
            {
                return new DynamicHandlerRegistration(id, node);
            }
            return default;
        }

        public virtual void RemoveHandler(IMessageHandler<TClient, TMessage> handler)
        {

        }

        public virtual async Task HandleMessageAsync(TClient client, TMessage message)
        {
            foreach (IMessageHandler<TClient, TMessage> handler in this)
            {
                await handler.HandleMessageAsync(client, message);
                if (message.BlockRemainingHandlers)
                {
                    break;
                }
            }
        }

        public override Task HandleMessageAsync(IMessageClient client, IMessage message)
        {
            return HandleMessageAsync((TClient)client, (TMessage)message);
        }
    }
}
