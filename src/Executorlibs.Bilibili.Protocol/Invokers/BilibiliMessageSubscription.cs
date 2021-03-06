using System.Collections.Generic;
using Executorlibs.Bilibili.Protocol.Clients;
using Executorlibs.Bilibili.Protocol.Handlers;
using Executorlibs.Bilibili.Protocol.Models.General;
using Executorlibs.MessageFramework.Handlers;
using Executorlibs.MessageFramework.Invoking;

namespace Executorlibs.Bilibili.Protocol.Invokers
{
    public class BilibiliMessageSubscription<TMessage> : MessageSubscription<IDanmakuClient, TMessage>, IBilibiliMessageSubscription<TMessage> where TMessage : IBilibiliMessage
    {
        public BilibiliMessageSubscription(IEnumerable<IBilibiliMessageHandler> handlers) : base(handlers)
        {

        }

        protected override IMessageHandler[] ResolveStaticHandlers(LinkedList<IMessageHandler> handlers, List<IMessageHandler> filtered)
        {
            if (handlers.Count != 0)
            {
                for (LinkedListNode<IMessageHandler>? handlerNode = handlers.First; handlerNode != null; handlerNode = handlerNode.Next)
                {
                    IMessageHandler handler = handlerNode.Value;
                    if (handler is IContravarianceBilibiliMessageHandler<TMessage> ||
                        handler is IInvarianceBilibiliMessageHandler<TMessage>)
                    {
                        filtered.Add(handler);
                        handlers.Remove(handlerNode);
                    }
                }
            }
            return base.ResolveStaticHandlers(handlers, filtered);
        }

#if NETSTANDARD2_0
        public virtual System.Threading.Tasks.Task HandleMessageAsync(IDanmakuClient session, IBilibiliMessage message)
        {
            return base.HandleMessageAsync(client: session, (TMessage)message);
        }
#endif
    }
}
