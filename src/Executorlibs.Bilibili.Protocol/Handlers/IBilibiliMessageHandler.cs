using System.Threading.Tasks;
using Executorlibs.Bilibili.Protocol.Clients;
using Executorlibs.Bilibili.Protocol.Models.General;
using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Handlers;
using Executorlibs.MessageFramework.Models.General;

namespace Executorlibs.Bilibili.Protocol.Handlers
{
    public interface IBilibiliMessageHandler : IMessageHandler
    {
        Task HandleMessage(IDanmakuClient client, IBilibiliMessage message)
            => HandleMessage(client, (IMessage)message);
    }

    public interface IBilibiliMessageHandler<in TMessage> : IMessageHandler<TMessage>, IBilibiliMessageHandler where TMessage : IBilibiliMessage // Should not impl this interface directly
    {
        Task HandleMessage(IDanmakuClient client, TMessage message);

        Task IMessageHandler<TMessage>.HandleMessage(IMessageClient client, TMessage message)
            => HandleMessage((IDanmakuClient)client, message);
    }
}
