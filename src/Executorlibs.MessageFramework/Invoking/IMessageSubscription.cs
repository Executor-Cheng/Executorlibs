using System;
using System.Collections.Generic;
using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Handlers;
using Executorlibs.MessageFramework.Models.General;
#if !NETSTANDARD2_0
using System.Threading.Tasks;
#endif

namespace Executorlibs.MessageFramework.Invoking
{
    public interface IMessageSubscription : IMessageHandler, IDisposable, IEnumerable<IMessageHandler>
    {
        DynamicHandlerRegistration AddHandler(IMessageHandler handler);
    }

    public interface IMessageSubscription<TClient, TMessage> : IMessageSubscription,
                                                               IMessageHandler<TClient, TMessage> where TClient : IMessageClient // 只允许实现一种泛型接口
                                                                                                  where TMessage : IMessage // 想实现多种的请自己解决CS8705
    {
        DynamicHandlerRegistration AddHandler(IMessageHandler<TClient, TMessage> handler);

#if !NETSTANDARD2_0
        Task IMessageHandler.HandleMessageAsync(IMessageClient client, IMessage message)
            => HandleMessageAsync((TClient)client, (TMessage)message);
#endif
    }
}
