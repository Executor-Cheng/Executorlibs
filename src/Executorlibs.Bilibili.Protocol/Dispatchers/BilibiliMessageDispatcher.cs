using Executorlibs.Bilibili.Protocol.Clients;
using Executorlibs.Bilibili.Protocol.Models.General;
using Executorlibs.Bilibili.Protocol.Subscriptions;
using Executorlibs.MessageFramework.Dispatchers;

namespace Executorlibs.Bilibili.Protocol.Dispatchers
{
    public interface IBilibiliMessageDispatcher<TMessage> : IMessageDispatcher<IDanmakuClient, TMessage> where TMessage : IBilibiliMessage
    {

    }

    public class BilibiliMessageDispatcher<TMessage> : DefaultMessageDispatcher<IDanmakuClient, TMessage>, IBilibiliMessageDispatcher<TMessage> where TMessage : IBilibiliMessage
    {
        public BilibiliMessageDispatcher(IBilibiliMessageSubscription<TMessage> subscription) : base(subscription)
        {

        }
    }
}
