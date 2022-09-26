using System.Threading.Tasks;
using Executorlibs.MessageFramework.Models.General;

namespace Executorlibs.MessageFramework.Clients
{
    public interface IMessageDispatchClient<TMessage> : IMessageClient where TMessage : IMessage
    {
        Task DispatchAsync<TRelatedMessage>(TRelatedMessage message) where TRelatedMessage : TMessage;
    }

    public abstract class MessageDispatchClient<TMessage> : MessageClient, IMessageDispatchClient<TMessage> where TMessage : IMessage
    {
        public abstract Task DispatchAsync<TRelatedMessage>(TRelatedMessage message) where TRelatedMessage : TMessage;
    }
}
