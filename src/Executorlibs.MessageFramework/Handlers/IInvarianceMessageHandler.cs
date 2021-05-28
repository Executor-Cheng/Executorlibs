using Executorlibs.MessageFramework.Models.General;

namespace Executorlibs.MessageFramework.Handlers
{
    public interface IInvarianceMessageHandler<TMessage> : IMessageHandler<TMessage> where TMessage : IMessage
    {

    }
}
