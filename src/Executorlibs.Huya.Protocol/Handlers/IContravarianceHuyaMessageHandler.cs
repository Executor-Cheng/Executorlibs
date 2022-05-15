using Executorlibs.Huya.Protocol.Clients;
using Executorlibs.Huya.Protocol.Models.General;
using Executorlibs.MessageFramework.Handlers;

namespace Executorlibs.Huya.Protocol.Handlers
{
    public interface IContravarianceHuyaMessageHandler<in TMessage> : IHuyaMessageHandler<TMessage>, IContravarianceMessageHandler<IHuyaClient, TMessage> where TMessage : IHuyaMessage
    {

    }
}
