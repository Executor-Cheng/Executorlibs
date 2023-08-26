using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Models.General;
using System.Threading.Tasks;

namespace Executorlibs.MessageFramework.Handlers
{
    public interface IMessageHandler<in TClient, in TMessage> where TClient : IMessageClient
                                                              where TMessage : IMessage
    {
        Task HandleMessageAsync(TClient client, TMessage message);
    }

    public abstract class MessageHandler<TClient, TMessage> : IMessageHandler<TClient, TMessage> where TClient : IMessageClient
                                                                                                 where TMessage : IMessage
    {
        public abstract Task HandleMessageAsync(TClient client, TMessage message);
    }
}
