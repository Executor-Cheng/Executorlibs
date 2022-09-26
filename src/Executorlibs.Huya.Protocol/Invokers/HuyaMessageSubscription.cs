using System.Collections.Generic;
using Executorlibs.Huya.Protocol.Clients;
using Executorlibs.Huya.Protocol.Handlers;
using Executorlibs.Huya.Protocol.Models.General;
using Executorlibs.MessageFramework.Handlers;
using Executorlibs.MessageFramework.Invoking;

namespace Executorlibs.Huya.Protocol.Invokers
{
    public class HuyaMessageSubscription<TMessage> : MessageSubscription<IHuyaClient, TMessage>, IHuyaMessageSubscription<TMessage> where TMessage : IHuyaMessage
    {
        public HuyaMessageSubscription(IEnumerable<IHuyaMessageHandler> handlers) : base(handlers)
        {

        }

        protected override IMessageHandler[] ResolveStaticHandlers(LinkedList<IMessageHandler> handlers, List<IMessageHandler> filtered)
        {
            if (handlers.Count != 0)
            {
                for (LinkedListNode<IMessageHandler>? handlerNode = handlers.First; handlerNode != null; handlerNode = handlerNode.Next)
                {
                    IMessageHandler handler = handlerNode.Value;
                    if (handler is IContravarianceHuyaMessageHandler<TMessage> ||
                        handler is IInvarianceHuyaMessageHandler<TMessage>)
                    {
                        filtered.Add(handler);
                        handlers.Remove(handlerNode);
                    }
                }
            }
            return base.ResolveStaticHandlers(handlers, filtered);
        }

#if NETSTANDARD2_0
        public virtual System.Threading.Tasks.Task HandleMessageAsync(IHuyaClient session, IHuyaMessage message)
        {
            return base.HandleMessageAsync(client: session, (TMessage)message);
        }
#endif
    }
}
