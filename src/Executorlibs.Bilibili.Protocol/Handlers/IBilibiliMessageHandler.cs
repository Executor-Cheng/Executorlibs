using System.Threading.Tasks;
using Executorlibs.Bilibili.Protocol.Clients;
using Executorlibs.Bilibili.Protocol.Models.General;
using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Handlers;
#if !NETSTANDARD2_0
using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Models.General;
#endif

namespace Executorlibs.Bilibili.Protocol.Handlers
{
    public interface IBilibiliMessageHandler : IMessageHandler // Should not impl this interface directly
    {
        Task HandleMessage(IDanmakuClient client, IBilibiliMessage message)
#if !NETSTANDARD2_0
            => HandleMessage(client, (IMessage)message)
#endif
            ;
    }

    public interface IBilibiliMessageHandler<in TMessage> : IMessageHandler<TMessage>, IBilibiliMessageHandler where TMessage : IBilibiliMessage
    {
        Task HandleMessage(IDanmakuClient client, TMessage message);

#if !NETSTANDARD2_0
        Task IMessageHandler<TMessage>.HandleMessage(IMessageClient client, TMessage message)
            => HandleMessage((IDanmakuClient)client, message);
#endif
    }

    public abstract class BilibiliMessageHandler<TMessage> : MessageHandler<TMessage>, IBilibiliMessageHandler<TMessage> where TMessage : IBilibiliMessage
    {
#if NETSTANDARD2_0

#endif
        public abstract Task HandleMessage(IDanmakuClient client, TMessage message);

        public virtual Task HandleMessage(IDanmakuClient client, IBilibiliMessage message)
        {
            return base.HandleMessage(client, message);
        }

        public override Task HandleMessage(IMessageClient client, TMessage message)
        {
            return HandleMessage((IDanmakuClient)client, message);
        }
    }
}
