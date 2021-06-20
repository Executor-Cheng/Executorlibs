using System.Collections.Generic;
using System.Threading.Tasks;
using Executorlibs.Bilibili.Protocol.Clients;
using Executorlibs.Bilibili.Protocol.Handlers;
using Executorlibs.Bilibili.Protocol.Models.General;
using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Handlers;
using Executorlibs.MessageFramework.Invoking;

namespace Executorlibs.Bilibili.Protocol.Invokers
{
    public class BilibiliMessageSubscription<TMessage> : MessageSubscription<TMessage>, IBilibiliMessageSubscription<TMessage> where TMessage : IBilibiliMessage
    {
#if NET5_0_OR_GREATER
        protected override IEnumerable<IBilibiliMessageHandler<TMessage>> Handlers => (IEnumerable<IBilibiliMessageHandler<TMessage>>)base.Handlers;
#endif

        public BilibiliMessageSubscription(IEnumerable<IBilibiliMessageHandler> handlers) : base(handlers)
        {

        }

#if NET5_0_OR_GREATER
        protected override IEnumerable<IBilibiliMessageHandler<TMessage>> ResolveHandlers(IEnumerable<IMessageHandler> handlers)
#else
        protected override IEnumerable<IMessageHandler<TMessage>> ResolveHandlers(IEnumerable<IMessageHandler> handlers)
#endif
        {
            List<IBilibiliMessageHandler<TMessage>> filteredHandlers = new List<IBilibiliMessageHandler<TMessage>>();
            var expectedHandler = typeof(IContravarianceBilibiliMessageHandler<TMessage>);
            var expectedInvarianceHandler = typeof(IInvarianceBilibiliMessageHandler<TMessage>);
            foreach (IMessageHandler handler in handlers)
            {
                if (expectedHandler.IsAssignableFrom(handler.GetType()) ||
                    expectedInvarianceHandler.IsAssignableFrom(handler.GetType()))
                {
                    filteredHandlers.Add((IBilibiliMessageHandler<TMessage>)handler);
                }
            }
            return filteredHandlers.ToArray();
        }

        public override Task HandleMessage(IMessageClient client, TMessage message)
        {
            return this.HandleMessage((IDanmakuClient)client, message);
        }

        public async Task HandleMessage(IDanmakuClient client, TMessage message)
        {
            foreach (IBilibiliMessageHandler<TMessage> handler in Handlers)
            {
                await handler.HandleMessage(client, message);
            }
        }

#if NETSTANDARD2_0
        public Task HandleMessage(IDanmakuClient client, IBilibiliMessage message)
        {
            return base.HandleMessage(client, (TMessage)message);
        }
#endif
    }
}
