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
        Task IBilibiliMessageHandler.HandleMessage(IDanmakuClient client, IBilibiliMessage message)
            => HandleMessage(client, (TMessage)message);
    }
}
