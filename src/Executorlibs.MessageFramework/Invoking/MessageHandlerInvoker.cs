using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Handlers;
using Executorlibs.MessageFramework.Models.General;
using Executorlibs.MessageFramework.Parsers;
using Microsoft.Extensions.Logging;

namespace Executorlibs.MessageFramework.Invoking
{
    public interface IMessageHandlerInvoker<TClientService> where TClientService : IMessageClient
    {
        Task HandleMessageAsync<TMessage>(TClientService client, TMessage message) where TMessage : IMessage;
    }

    public interface IMessageHandlerInvoker<TClientService, TRawdata> : IMessageHandlerInvoker<TClientService> where TClientService : IMessageClient
    {
        Task HandleRawdataAsync(TClientService client, TRawdata rawdata);
    }

    public abstract class MessageHandlerInvoker<TClientService, THandlerService> : IMessageHandlerInvoker<TClientService> where TClientService : IMessageClient
                                                                                                                          where THandlerService : IMessageHandler

    {
        protected readonly ConcurrentDictionary<Type, Type> _SubscriptionMapping = new();

        protected readonly IServiceProvider _Services;

        protected readonly ILogger<MessageHandlerInvoker<TClientService, THandlerService>> _Logger;

        protected MessageHandlerInvoker(IServiceProvider services, ILogger<MessageHandlerInvoker<TClientService, THandlerService>> logger)
        {
            _Services = services;
            _Logger = logger;
        }

        public virtual Task HandleMessageAsync<TMessage>(TClientService client, TMessage message) where TMessage : IMessage
        {
            if (TryResolveSubscription(out IMessageSubscription<TMessage>? subscription))
            {
                return subscription.HandleMessage(client, message);
            }
            return Task.CompletedTask;
        }

        protected virtual bool TryResolveSubscription<TMessage>([NotNullWhen(true)] out IMessageSubscription<TMessage>? subscription) where TMessage : IMessage
        {
            Unsafe.SkipInit(out subscription);
            ref IMessageSubscription? s = ref Unsafe.As<IMessageSubscription<TMessage>?, IMessageSubscription?>(ref subscription);
            return TryResolveSubscription(typeof(TMessage), out s);
        }

        protected virtual bool TryResolveSubscription(Type messageType, [NotNullWhen(true)] out IMessageSubscription? subscription)
        {
            if (!_SubscriptionMapping.TryGetValue(messageType, out Type? subscriptionType))
            {
                _SubscriptionMapping[messageType] = subscriptionType = typeof(IMessageSubscription<>).MakeGenericType(messageType);
            }
            Unsafe.SkipInit(out subscription);
            ref object? t = ref Unsafe.As<IMessageSubscription?, object?>(ref subscription);
            return (t = _Services.GetService(subscriptionType)) != null;
        }

    }

    public abstract class MessageHandlerInvoker<TClientService, THandlerService, TRawdata, TParserService> : MessageHandlerInvoker<TClientService, THandlerService>, IMessageHandlerInvoker<TClientService, TRawdata>
        where TParserService : IMessageParser<TRawdata>
        where THandlerService : IMessageHandler
        where TClientService : IMessageClient
    {
        protected MessageHandlerInvoker(IServiceProvider services, ILogger<MessageHandlerInvoker<TClientService, THandlerService, TRawdata, TParserService>> logger) : base(services, logger)
        {

        }

        public virtual async Task HandleRawdataAsync(TClientService client, TRawdata rawdata)
        {
            if (TryResolveParsers(in rawdata, out var parsers))
            {
                foreach (var parser in parsers)
                {
                    if (parser.CanParse(in rawdata))
                    {
                        IMessage<TRawdata> message = parser.Parse(in rawdata);
                        if (TryResolveSubscription(parser.MessageType, out IMessageSubscription? subscription))
                        {
                            await subscription.HandleMessage(client, message);
                        }
                    }
                }
            }
        }

        protected abstract bool TryResolveParsers(in TRawdata rawdata, [NotNullWhen(true)] out IEnumerable<TParserService>? parsers);
    }
}
