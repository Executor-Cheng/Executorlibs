using System;
using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Handlers;
using Executorlibs.MessageFramework.Models.General;

namespace Executorlibs.MessageFramework.Dispatchers
{
    public interface IMessageDispatcher<TClient, TMessage> : IMessageHandler<TClient, TMessage> where TClient : IMessageClient
                                                                                                where TMessage : IMessage
    {
        bool IsEmpty { get; }

        IDisposable AddHandler(IMessageHandler<TClient, TMessage> handler);
    }

    public abstract class MessageDispatcher<TClient, TMessage> : MessageHandler<TClient, TMessage>,
                                                                 IMessageDispatcher<TClient, TMessage> where TClient : IMessageClient where TMessage : IMessage
    {
        public abstract bool IsEmpty { get; }

        protected MessageDispatcher()
        {

        }

        public abstract IDisposable AddHandler(IMessageHandler<TClient, TMessage> handler);
    }
}
