using Executorlibs.MessageFramework.Models.General;

namespace Executorlibs.MessageFramework.Handlers
{
    public interface IContravarianceMessageHandler<in TMessage> : IMessageHandler<TMessage> where TMessage : IMessage
    {
        
    }
}
