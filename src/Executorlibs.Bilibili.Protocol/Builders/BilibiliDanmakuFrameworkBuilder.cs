using System;
using System.Reflection;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Clients;
using Executorlibs.Bilibili.Protocol.Handlers;
using Executorlibs.Bilibili.Protocol.Invokers;
using Executorlibs.Bilibili.Protocol.Invokers.Attributes;
using Executorlibs.Bilibili.Protocol.Options;
using Executorlibs.Bilibili.Protocol.Parsers;
using Executorlibs.Bilibili.Protocol.Parsers.Attributes;
using Executorlibs.Bilibili.Protocol.Services;
using Executorlibs.MessageFramework.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.Bilibili.Protocol.Builders
{
    // see runtime #47007, resolved in 5.0.4
    public class BilibiliDanmakuFrameworkBuilder
        : MessageFrameworkBuilder<IBilibiliMessageHandlerInvoker, IDanmakuClient, IBilibiliMessageHandler, JsonElement, IBilibiliMessageParser>
    {
        protected virtual ServiceLifetime DefaultCredentialProviderLifetime => ServiceLifetime.Scoped;

        public BilibiliDanmakuFrameworkBuilder(IServiceCollection services) : base(services)
        {

        }

        public virtual BilibiliDanmakuFrameworkBuilder AddCredentialProvider<TProvider>() where TProvider : class, IDanmakuServerProvider
        {
            return this.AddCredentialProvider<TProvider>(DefaultCredentialProviderLifetime);
        }

        public virtual BilibiliDanmakuFrameworkBuilder AddCredentialProvider<TProvider>(ServiceLifetime lifetime) where TProvider : class, IDanmakuServerProvider
        {
            AddService(typeof(IDanmakuServerProvider), typeof(TProvider), lifetime);
            return this;
        }

        public virtual BilibiliDanmakuFrameworkBuilder AddCredentialProvider(IDanmakuServerProvider providerInstance)
        {
            AddService(typeof(IDanmakuServerProvider), providerInstance);
            return this;
        }

        public virtual BilibiliDanmakuFrameworkBuilder AddCredentialProvider<TProvider>(Func<IServiceProvider, TProvider> factory) where TProvider : class, IDanmakuServerProvider
        {
            return this.AddCredentialProvider(factory, DefaultCredentialProviderLifetime);
        }

        public virtual BilibiliDanmakuFrameworkBuilder AddCredentialProvider<TProvider>(Func<IServiceProvider, TProvider> factory, ServiceLifetime lifetime) where TProvider : class, IDanmakuServerProvider
        {
            AddService(typeof(IDanmakuServerProvider), factory, lifetime);
            return this;
        }

        internal BilibiliDanmakuFrameworkBuilder AddDanmakuOptions()
        {
            Services.AddOptions<DanmakuClientOptions>().Configure(options => options.HeartbeatInterval = TimeSpan.FromSeconds(30));
            return this;
        }

#if NET5_0_OR_GREATER
        public override BilibiliDanmakuFrameworkBuilder AddClient<TClient>()
        {
            return (BilibiliDanmakuFrameworkBuilder)base.AddClient<TClient>();
        }

        public override BilibiliDanmakuFrameworkBuilder AddClient<TClient>(ServiceLifetime lifetime)
        {
            return (BilibiliDanmakuFrameworkBuilder)base.AddClient<TClient>(lifetime);
        }

        public override BilibiliDanmakuFrameworkBuilder AddClient(IDanmakuClient clientInstance)
        {
            return (BilibiliDanmakuFrameworkBuilder)base.AddClient(clientInstance);
        }

        public override BilibiliDanmakuFrameworkBuilder AddClient<TClient>(Func<IServiceProvider, TClient> factory)
        {
            return (BilibiliDanmakuFrameworkBuilder)base.AddClient(factory);
        }

        public override BilibiliDanmakuFrameworkBuilder AddClient<TClient>(Func<IServiceProvider, TClient> factory, ServiceLifetime lifetime)
        {
            return (BilibiliDanmakuFrameworkBuilder)base.AddClient(factory, lifetime);
        }

        public override BilibiliDanmakuFrameworkBuilder AddHandler<THandler>()
        {
            return (BilibiliDanmakuFrameworkBuilder)base.AddHandler<THandler>();
        }

        public override BilibiliDanmakuFrameworkBuilder AddHandler<THandler>(ServiceLifetime lifetime)
        {
            return (BilibiliDanmakuFrameworkBuilder)base.AddHandler<THandler>(lifetime);
        }

        public override BilibiliDanmakuFrameworkBuilder AddHandler(IBilibiliMessageHandler handlerInstance)
        {
            return (BilibiliDanmakuFrameworkBuilder)base.AddHandler(handlerInstance);
        }

        public override BilibiliDanmakuFrameworkBuilder AddHandler<THandler>(Func<IServiceProvider, THandler> factory)
        {
            return (BilibiliDanmakuFrameworkBuilder)base.AddHandler(factory);
        }

        public override BilibiliDanmakuFrameworkBuilder AddHandler<THandler>(Func<IServiceProvider, THandler> factory, ServiceLifetime lifetime)
        {
            return (BilibiliDanmakuFrameworkBuilder)base.AddHandler(factory, lifetime);
        }

        public override BilibiliDanmakuFrameworkBuilder AddInvoker<TInvoker>()
        {
            return (BilibiliDanmakuFrameworkBuilder)base.AddInvoker<TInvoker>();
        }

        public override BilibiliDanmakuFrameworkBuilder AddInvoker<TInvoker>(ServiceLifetime lifetime)
        {
            return (BilibiliDanmakuFrameworkBuilder)base.AddInvoker<TInvoker>(lifetime);
        }

        public override BilibiliDanmakuFrameworkBuilder AddInvoker(IBilibiliMessageHandlerInvoker invokerInstance)
        {
            return (BilibiliDanmakuFrameworkBuilder)base.AddInvoker(invokerInstance);
        }

        public override BilibiliDanmakuFrameworkBuilder AddInvoker<TInvoker>(Func<IServiceProvider, TInvoker> factory)
        {
            return (BilibiliDanmakuFrameworkBuilder)base.AddInvoker(factory);
        }

        public override BilibiliDanmakuFrameworkBuilder AddInvoker<TInvoker>(Func<IServiceProvider, TInvoker> factory, ServiceLifetime lifetime)
        {
            return (BilibiliDanmakuFrameworkBuilder)base.AddInvoker(factory, lifetime);
        }

        public override BilibiliDanmakuFrameworkBuilder AddParser<TParser>()
        {
            return (BilibiliDanmakuFrameworkBuilder)base.AddParser<TParser>();
        }

        public override BilibiliDanmakuFrameworkBuilder AddParser<TParser>(ServiceLifetime lifetime)
        {
            return (BilibiliDanmakuFrameworkBuilder)base.AddParser<TParser>(lifetime);
        }

        public override BilibiliDanmakuFrameworkBuilder AddParser(IBilibiliMessageParser parserInstance)
        {
            return (BilibiliDanmakuFrameworkBuilder)base.AddParser(parserInstance);
        }

        public override BilibiliDanmakuFrameworkBuilder AddParser<TParser>(Func<IServiceProvider, TParser> factory)
        {
            return (BilibiliDanmakuFrameworkBuilder)base.AddParser(factory);
        }

        public override BilibiliDanmakuFrameworkBuilder AddParser<TParser>(Func<IServiceProvider, TParser> factory, ServiceLifetime lifetime)
        {
            return (BilibiliDanmakuFrameworkBuilder)base.AddParser(factory, lifetime);
        }
#endif
        protected override void ResolveMessageParser(Type handlerType, ServiceLifetime? lifetime)
        {
            foreach (RegisterBilibiliParserAttribute attribute in handlerType.GetCustomAttributes<RegisterBilibiliParserAttribute>())
            {
                AddService(attribute.ServiceType, attribute.ImplementationType, lifetime ?? attribute.Lifetime ?? DefaultParserLifetime);
            }
        }

        protected override void ResolveMessageSubscription(Type invokerType, ServiceLifetime? lifetime)
        {
            foreach (RegisterBilibiliMessageSubscriptionAttribute attribute in invokerType.GetCustomAttributes<RegisterBilibiliMessageSubscriptionAttribute>())
            {
                AddService(attribute.ServiceType, attribute.ImplementationType, lifetime ?? attribute.Lifetime ?? DefaultSubscriptionLifetime);
            }
        }
    }

    public static class BilibiliDanmakuFrameworkServiceCollectionExtensions
    {
        public static BilibiliDanmakuFrameworkBuilder AddBilibiliDanmakuFramework(this IServiceCollection services)
        {
            return new BilibiliDanmakuFrameworkBuilder(services)
                .AddDanmakuOptions();
        }
    }
}
