using Executorlibs.Bilibili.Protocol.Clients;
using Executorlibs.Bilibili.Protocol.Models.General;
using Executorlibs.MessageFramework.Handlers;

namespace Executorlibs.Bilibili.Protocol.Handlers
{
    public interface IBilibiliMessageHandler<in TMessage> : IMessageHandler<IDanmakuClient, TMessage> where TMessage : IBilibiliMessage
    {

    }

    public abstract class BilibiliMessageHandler<TMessage> : MessageHandler<IDanmakuClient, TMessage>, IBilibiliMessageHandler<TMessage> where TMessage : IBilibiliMessage
    {

    }
}
