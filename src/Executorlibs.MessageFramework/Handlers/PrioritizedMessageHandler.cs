using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Models.General;

namespace Executorlibs.MessageFramework.Handlers
{
    public interface IPrioritizedMessageHandler<in TClient, in TMessage> : IMessageHandler<TClient, TMessage> where TClient : IMessageClient
                                                                                                              where TMessage : IMessage
    {
        int Priority { get; }
    }

    public abstract class PrioritizedMessageHandler<TClient, TMessage> : MessageHandler<TClient, TMessage>, IPrioritizedMessageHandler<TClient, TMessage>
        where TClient : IMessageClient
        where TMessage : IMessage
    {
        public abstract int Priority { get; }
    }
}
