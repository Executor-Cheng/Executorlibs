using System;
using System.Threading.Tasks;
using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Models.General;

namespace Executorlibs.MessageFramework.Handlers
{
    public interface IMessageHandler
    {
#if !NETSTANDARD2_0
        protected static readonly Task _DefaultImplTask = Task.FromException(new NotSupportedException("请使用泛型接口中的HandleMessageAsync方法。"));

        Task HandleMessage(IMessageClient client, IMessage message)
        {
            return _DefaultImplTask;
        }
#else
        Task HandleMessage(IMessageClient client, IMessage message);
#endif
    }
#if !NETSTANDARD2_0
    [Obsolete("在此目标框架进行抽象时强烈不建议继承此类, 建议实现 IMessageHandler 接口")]
#endif
    public abstract class MessageHandler : IMessageHandler
    {
#if !NETSTANDARD2_0
        public virtual Task HandleMessage(IMessageClient client, IMessage message)
        {
            return IMessageHandler._DefaultImplTask;
        }
#else
        protected static readonly Task _DefaultImplTask = Task.FromException(new NotSupportedException("请使用泛型接口中的HandleMessageAsync方法。"));

        public virtual Task HandleMessage(IMessageClient client, IMessage message)
        {
            return _DefaultImplTask;
        }
#endif
    }

    public interface IMessageHandler<in TMessage> : IMessageHandler where TMessage : IMessage
    {
        Task HandleMessage(IMessageClient client, TMessage message);
    }

#if !NETSTANDARD2_0
    [Obsolete("在此目标框架进行抽象时强烈不建议继承此类, 建议实现 IMessageHandler<in TMessage> 接口")]
#endif
    public abstract class MessageHandler<TMessage> : MessageHandler, IMessageHandler<TMessage> where TMessage : IMessage
    {
        public abstract Task HandleMessage(IMessageClient client, TMessage message);

        public override Task HandleMessage(IMessageClient client, IMessage message)
        {
            return HandleMessage(client, (TMessage)message);
        }
    }
}
