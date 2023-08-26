using System.Threading.Tasks;
using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Models.General;

namespace Executorlibs.MessageFramework.Dispatchers
{
    public interface IMessageDispatcher<in TClient, in TMessage> where TClient : IMessageClient where TMessage : IMessage
    {
        Task HandleMessageAsync(TClient client, TMessage message);
    }

    public abstract class MessageDispatcher<TClient, TMessage> : IMessageDispatcher<TClient, TMessage> where TClient : IMessageClient where TMessage : IMessage
    {
        protected MessageDispatcher()
        {

        }

        public abstract Task HandleMessageAsync(TClient client, TMessage message);
    }
}
