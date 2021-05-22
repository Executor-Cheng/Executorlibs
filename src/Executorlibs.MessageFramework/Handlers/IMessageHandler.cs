using System;
using System.Threading.Tasks;
using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Models.General;

namespace Executorlibs.MessageFramework.Handlers
{
    public interface IMessageHandler
    {
        protected static readonly Task _DefaultImplTask = Task.FromException(new NotSupportedException("请使用泛型接口中的HandleMessageAsync方法。"));

        Task HandleMessage(IMessageClient client, IMessage message)
            => _DefaultImplTask;
    }

    public interface IMessageHandler<in TMessage> : IMessageHandler where TMessage : IMessage
    {
        Task HandleMessage(IMessageClient client, TMessage message);
    }
}
