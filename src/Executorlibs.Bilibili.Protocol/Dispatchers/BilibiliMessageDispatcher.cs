using Executorlibs.Bilibili.Protocol.Clients;
using Executorlibs.Bilibili.Protocol.Models.General;
using Executorlibs.MessageFramework.Dispatchers;
using Executorlibs.MessageFramework.Subscriptions;

namespace Executorlibs.Bilibili.Protocol.Dispatchers
{
    public interface IBilibiliMessageDispatcher<TMessage> : IMessageDispatcher<IDanmakuClient, TMessage> where TMessage : IBilibiliMessage
    {

    }

    public class BilibiliMessageDispatcher<TMessage> : DefaultMessageDispatcher<IDanmakuClient, TMessage>, IBilibiliMessageDispatcher<TMessage> where TMessage : IBilibiliMessage
    {
        public BilibiliMessageDispatcher(IMessageSubscription<IDanmakuClient, TMessage> subscription) : base(subscription)
        {

        }
    }
}
