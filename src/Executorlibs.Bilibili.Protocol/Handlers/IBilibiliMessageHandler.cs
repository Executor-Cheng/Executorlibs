using System.Threading.Tasks;
using Executorlibs.Bilibili.Protocol.Clients;
using Executorlibs.Bilibili.Protocol.Models.General;
using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Handlers;
using Executorlibs.MessageFramework.Models.General;

namespace Executorlibs.Bilibili.Protocol.Handlers
{
    public interface IBilibiliMessageHandler : IMessageHandler // Should not impl this interface directly
    {
#if !NETSTANDARD2_0
        Task HandleMessageAsync(IDanmakuClient client, IBilibiliMessage message)
        {
            return _DefaultImplTask;
        }

        Task IMessageHandler.HandleMessageAsync(IMessageClient session, IMessage message)
        {
            return HandleMessageAsync((IDanmakuClient)session, (IBilibiliMessage)message);
        }
#else
        Task HandleMessageAsync(IDanmakuClient client, IBilibiliMessage message);
#endif
    }

    public interface IBilibiliMessageHandler<in TMessage> : IMessageHandler<IDanmakuClient, TMessage>, IBilibiliMessageHandler where TMessage : IBilibiliMessage
    {
        
    }

    public abstract class BilibiliMessageHandler<TMessage> : MessageHandler<IDanmakuClient, TMessage>, IBilibiliMessageHandler<TMessage> where TMessage : IBilibiliMessage
    {
        public override Task HandleMessageAsync(IMessageClient client, IMessage message)
        {
            return HandleMessageAsync((IDanmakuClient)client, (IBilibiliMessage)message);
        }

#if NETSTANDARD2_0
        public virtual Task HandleMessageAsync(IDanmakuClient client, IBilibiliMessage message)
        {
            return Task.FromException(new System.NotSupportedException("请使用泛型接口中的HandleMessageAsync方法。"));
        }
#endif
    }
}
