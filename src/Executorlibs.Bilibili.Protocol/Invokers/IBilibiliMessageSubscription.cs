using System.Threading.Tasks;
using Executorlibs.Bilibili.Protocol.Clients;
using Executorlibs.Bilibili.Protocol.Handlers;
using Executorlibs.Bilibili.Protocol.Models.General;
using Executorlibs.MessageFramework.Invoking;

namespace Executorlibs.Bilibili.Protocol.Invokers
{
    public interface IBilibiliMessageSubscription : IMessageSubscription, IBilibiliMessageHandler
    {

    }

    public interface IBilibiliMessageSubscription<TMessage> : IBilibiliMessageSubscription, IBilibiliMessageHandler<TMessage> where TMessage : IBilibiliMessage
    {
#if !NETSTANDARD2_0
        Task IBilibiliMessageHandler.HandleMessageAsync(IDanmakuClient client, IBilibiliMessage message)
            => HandleMessageAsync(client, (TMessage)message);
#endif
    }
}
