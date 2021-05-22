using Executorlibs.Bilibili.Protocol.Models.General;
using Executorlibs.MessageFramework.Handlers;

namespace Executorlibs.Bilibili.Protocol.Handlers
{
    public interface IInvarianceBilibiliMessageHandler<TMessage> : IBilibiliMessageHandler<TMessage>, IInvarianceMessageHandler<TMessage> where TMessage : IBilibiliMessage
    {
        
    }
}
