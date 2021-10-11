using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Models.General;

namespace Executorlibs.MessageFramework.Handlers
{
    public interface IContravarianceMessageHandler<in TClient, in TMessage> : IMessageHandler<TClient, TMessage> where TClient : IMessageClient
                                                                                                                 where TMessage : IMessage
    {

    }
}
