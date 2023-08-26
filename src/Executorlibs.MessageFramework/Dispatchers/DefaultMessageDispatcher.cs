using System.Threading.Tasks;
using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Models.General;
using Executorlibs.MessageFramework.Subscriptions;

namespace Executorlibs.MessageFramework.Dispatchers
{
    public class DefaultMessageDispatcher<TClient, TMessage> : MessageDispatcher<TClient, TMessage> where TClient : IMessageClient where TMessage : IMessage
    {
        protected readonly IMessageSubscription<TClient, TMessage> _subscription;

        public DefaultMessageDispatcher(IMessageSubscription<TClient, TMessage> subscription)
        {
            _subscription = subscription;
        }

        public override Task HandleMessageAsync(TClient client, TMessage message)
        {
            return _subscription.HandleMessageAsync(client, message);
        }
    }
}
