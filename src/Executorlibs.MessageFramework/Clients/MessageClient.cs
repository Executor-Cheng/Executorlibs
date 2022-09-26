using System;
using System.Collections.Generic;
using Executorlibs.MessageFramework.Handlers;
using Executorlibs.MessageFramework.Invoking;

namespace Executorlibs.MessageFramework.Clients
{
    public interface IMessageClient
    {
        PluginResistration AddPlugin(IMessageHandler handler);
    }

    public abstract class MessageClient : IMessageClient
    {
        protected abstract IEnumerable<IMessageSubscription> ResolveByHandler(Type handlerType);

        public PluginResistration AddPlugin(IMessageHandler handler)
        {
            LinkedList<DynamicHandlerRegistration> registrations = new LinkedList<DynamicHandlerRegistration>();
            foreach (IMessageSubscription subscription in ResolveByHandler(handler.GetType()))
            {
                registrations.AddLast(subscription.AddHandler(handler));
            }
            return new PluginResistration(registrations);
        }
    }
}
