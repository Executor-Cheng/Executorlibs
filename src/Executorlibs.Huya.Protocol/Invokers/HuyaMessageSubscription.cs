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
                var expectedHandler = typeof(IContravarianceHuyaMessageHandler<TMessage>);
                var expectedInvarianceHandler = typeof(IInvarianceHuyaMessageHandler<TMessage>);
                for (LinkedListNode<IMessageHandler>? handlerNode = handlers.First; handlerNode != null; handlerNode = handlerNode.Next)
                {
                    IMessageHandler handler = handlerNode.Value;
                    if (expectedHandler.IsAssignableFrom(handler.GetType()) ||
                        expectedInvarianceHandler.IsAssignableFrom(handler.GetType()))
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
