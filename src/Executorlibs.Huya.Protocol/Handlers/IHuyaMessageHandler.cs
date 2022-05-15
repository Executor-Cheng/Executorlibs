using System;
using System.Threading.Tasks;
using Executorlibs.Huya.Protocol.Clients;
using Executorlibs.Huya.Protocol.Models.General;
using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Handlers;
using Executorlibs.MessageFramework.Models.General;

namespace Executorlibs.Huya.Protocol.Handlers
{
    public interface IHuyaMessageHandler : IMessageHandler // Should not impl this interface directly
    {
#if !NETSTANDARD2_0
        Task HandleMessageAsync(IHuyaClient client, IHuyaMessage message)
        {
            throw new NotSupportedException("请使用泛型接口中的 HandleMessageAsync 方法。");
        }

        Task IMessageHandler.HandleMessageAsync(IMessageClient session, IMessage message)
        {
            return HandleMessageAsync((IHuyaClient)session, (IHuyaMessage)message);
        }
#else
        Task HandleMessageAsync(IHuyaClient client, IHuyaMessage message);
#endif
    }

    public interface IHuyaMessageHandler<in TMessage> : IMessageHandler<IHuyaClient, TMessage>, IHuyaMessageHandler where TMessage : IHuyaMessage
    {

    }

    public abstract class HuyaMessageHandler<TMessage> : MessageHandler<IHuyaClient, TMessage>, IHuyaMessageHandler<TMessage> where TMessage : IHuyaMessage
    {
        public override Task HandleMessageAsync(IMessageClient client, IMessage message)
        {
            return HandleMessageAsync((IHuyaClient)client, (IHuyaMessage)message);
        }

#if NETSTANDARD2_0
        public virtual Task HandleMessageAsync(IHuyaClient client, IHuyaMessage message)
        {
            return Task.FromException(new System.NotSupportedException("请使用泛型接口中的HandleMessageAsync方法。"));
        }
#endif
    }
}
