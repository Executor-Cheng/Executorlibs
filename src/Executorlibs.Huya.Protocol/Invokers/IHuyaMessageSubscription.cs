using System.Threading.Tasks;
using Executorlibs.Huya.Protocol.Clients;
using Executorlibs.Huya.Protocol.Handlers;
using Executorlibs.Huya.Protocol.Models.General;
using Executorlibs.MessageFramework.Invoking;

namespace Executorlibs.Huya.Protocol.Invokers
{
    public interface IHuyaMessageSubscription : IMessageSubscription, IHuyaMessageHandler
    {

    }

    public interface IHuyaMessageSubscription<TMessage> : IHuyaMessageSubscription, IHuyaMessageHandler<TMessage> where TMessage : IHuyaMessage
    {
#if !NETSTANDARD2_0
        Task IHuyaMessageHandler.HandleMessageAsync(IHuyaClient client, IHuyaMessage message)
            => HandleMessageAsync(client, (TMessage)message);
#endif
    }
}
