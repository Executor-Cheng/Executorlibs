using Executorlibs.MessageFramework.Handlers;
using Executorlibs.MessageFramework.Models.General;
#if !NETSTANDARD2_0
using System.Threading.Tasks;
using Executorlibs.MessageFramework.Clients;
#endif

namespace Executorlibs.MessageFramework.Invoking
{
    public interface IMessageSubscription : IMessageHandler
    {

    }

    public interface IMessageSubscription<TMessage> : IMessageSubscription, IMessageHandler<TMessage> where TMessage : IMessage // 只允许实现一种泛型接口
                                                                                                                                // 想实现多种的请自己解决CS8705
    {
#if !NETSTANDARD2_0
        Task IMessageHandler.HandleMessage(IMessageClient client, IMessage message)
            => HandleMessage(client, (TMessage)message);
#endif
    }
}
