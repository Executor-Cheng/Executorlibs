using Executorlibs.Huya.Protocol.Clients;
using Executorlibs.Huya.Protocol.Models.General;
using Executorlibs.MessageFramework.Handlers;

namespace Executorlibs.Huya.Protocol.Handlers
{
    public interface IInvarianceHuyaMessageHandler<TMessage> : IHuyaMessageHandler<TMessage>, IInvarianceMessageHandler<IHuyaClient, TMessage> where TMessage : IHuyaMessage
    {

    }
}
