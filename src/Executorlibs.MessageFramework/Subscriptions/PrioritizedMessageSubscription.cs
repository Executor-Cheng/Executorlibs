using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Handlers;
using Executorlibs.MessageFramework.Models.General;

namespace Executorlibs.MessageFramework.Subscriptions
{
    public class PrioritizedMessageSubscription<TClient, TMessage> : MessageSubscription<TClient, TMessage> where TClient : IMessageClient
                                                                                                            where TMessage : IMessage
    {
        [DebuggerDisplay("[{Priority}] {Hook}")]
        protected readonly struct HookEntry
        {
            public readonly int Priority;

            public readonly IMessageHandler<TClient, TMessage> Hook;

            public HookEntry(int priority, IMessageHandler<TClient, TMessage> hook)
            {
                Priority = priority;
                Hook = hook;
            }
        }

        protected HookEntry[] _handlers;

        protected readonly object _sync;

        public override bool IsEmpty => _handlers.Length == 0;

        public PrioritizedMessageSubscription(IEnumerable<IMessageHandler<TClient, TMessage>> handlers)
        {
            _handlers = Array.Empty<HookEntry>();
            _sync = new object();
            foreach (var handler in handlers)
            {
                var priority = GetPriority(handler);
                AddHandler(priority, handler);
            }
        }

        protected static int GetPriority(IMessageHandler<TClient, TMessage> handler)
        {
            return handler is IPrioritizedMessageHandler<TClient, TMessage> prioritized ? prioritized.Priority : 0;
        }

        public override IDisposable AddHandler(IMessageHandler<TClient, TMessage> handler)
        {
            var priority = GetPriority(handler);
            AddHandler(priority, handler);
            return new Registration(this, handler);
        }

        protected void AddHandler(int priority, IMessageHandler<TClient, TMessage> handler)
        {
            lock (_sync)
            {
                var handlers = _handlers;
                var newHandlers = new HookEntry[handlers.Length + 1];
                int i = 0, j = 0;
                while (j < handlers.Length)
                {
                    // >= 时后来的后处理, > 时后来的先处理
                    if (handler == null || priority >= handlers[j].Priority)
                    {
                        newHandlers[i] = handlers[j];
                        j++;
                    }
                    else
                    {
                        newHandlers[i] = new HookEntry(priority, handler);
                        handler = null!;
                    }
                    i++;
                }
                if (handler != null)
                {
                    newHandlers[i] = new HookEntry(priority, handler);
                }
                _handlers = newHandlers;
            }
        }

        protected void RemoveHandler(IMessageHandler<TClient, TMessage> handler)
        {
            lock (_sync)
            {
                var handlers = _handlers;
                for (int i = 0; i < handlers.Length; i++)
                {
                    if (handlers[i].Hook == handler)
                    {
                        handlers[i] = new HookEntry(int.MinValue, PassthroughHandler.Singleton);
                        break;
                    }
                }
            }
            ShrinkHandler();
        }

        protected void ShrinkHandler()
        {
            lock (_sync)
            {
                var handlers = _handlers;
                for (int i = 0; i < handlers.Length; i++)
                {
                    if (handlers[i].Hook == PassthroughHandler.Singleton)
                    {
                        var newHandlers = new HookEntry[handlers.Length - 1];
                        Array.Copy(handlers, newHandlers, i);
                        var remaining = newHandlers.Length - i;
                        if (remaining != 0)
                        {
                            Array.Copy(handlers, i + 1, newHandlers, i, remaining);
                        }
                        _handlers = newHandlers;
                        return;
                    }
                }
            }
        }

        public override IEnumerator<IMessageHandler<TClient, TMessage>> GetEnumerator()
        {
            return _handlers.Select(p => p.Hook).GetEnumerator();
        }

        public override async Task HandleMessageAsync(TClient client, TMessage message)
        {
            foreach (var handler in _handlers)
            {
                await handler.Hook.HandleMessageAsync(client, message);
                if (message.BlockRemainingHandlers)
                {
                    break;
                }
            }
        }

        protected sealed class PassthroughHandler : MessageHandler<TClient, TMessage>
        {
            public static readonly PassthroughHandler Singleton = new PassthroughHandler();

            public override Task HandleMessageAsync(TClient client, TMessage message)
            {
                return Task.CompletedTask;
            }
        }

        protected class Registration : IDisposable
        {
            protected readonly PrioritizedMessageSubscription<TClient, TMessage> _subscription;

            protected IMessageHandler<TClient, TMessage>? _hook;

            public Registration(PrioritizedMessageSubscription<TClient, TMessage> subscription, IMessageHandler<TClient, TMessage> hook)
            {
                _subscription = subscription;
                _hook = hook;
            }

            public void Dispose()
            {
                var hook = Interlocked.Exchange(ref _hook, null);
                if (hook != null)
                {
                    _subscription.RemoveHandler(hook);
                }
            }
        }
    }
}
