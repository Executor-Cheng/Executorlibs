using System;
using System.Reflection;
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
        : MessageFrameworkBuilder<IBilibiliMessageHandlerInvoker, IDanmakuClient, IBilibiliMessageHandler, IBilibiliMessageParser>
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
            TryAddService(typeof(IDanmakuServerProvider), typeof(TProvider), lifetime, out _);
            return this;
        }

        public virtual BilibiliDanmakuFrameworkBuilder AddCredentialProvider(IDanmakuServerProvider providerInstance)
        {
            TryAddService(typeof(IDanmakuServerProvider), providerInstance, out _);
            return this;
        }

        public virtual BilibiliDanmakuFrameworkBuilder AddCredentialProvider<TProvider>(Func<IServiceProvider, TProvider> factory) where TProvider : class, IDanmakuServerProvider
        {
            return this.AddCredentialProvider(factory, DefaultCredentialProviderLifetime);
        }

        public virtual BilibiliDanmakuFrameworkBuilder AddCredentialProvider<TProvider>(Func<IServiceProvider, TProvider> factory, ServiceLifetime lifetime) where TProvider : class, IDanmakuServerProvider
        {
            TryAddService(typeof(IDanmakuServerProvider), factory, lifetime, out _);
            return this;
        }

        internal BilibiliDanmakuFrameworkBuilder AddDanmakuOptions()
        {
            Services.AddOptions<DanmakuClientOptions>().Configure(options => options.HeartbeatInterval = TimeSpan.FromSeconds(30));
            return this;
        }

        protected override void ResolveMessageParser(Type handlerType, ServiceLifetime? lifetime)
        {
            foreach (RegisterBilibiliParserAttribute attribute in handlerType.GetCustomAttributes<RegisterBilibiliParserAttribute>(false))
            {
                ServiceLifetime parserLifetime = lifetime ?? attribute.Lifetime ?? DefaultParserLifetime;
                AddMessageParser(attribute.ServiceType, attribute.ImplementationType, parserLifetime);
            }
        }

        protected override void ResolveMessageParserResolver(Type invokerType, ServiceLifetime? lifetime)
        {
            foreach (RegisterBilibiliMessageParserResolverAttribute attribute in invokerType.GetCustomAttributes<RegisterBilibiliMessageParserResolverAttribute>(false))
            {
                ServiceLifetime parserResolverLifetime = lifetime ?? attribute.Lifetime ?? DefaultParserResolverLifetime;
                AddMessageParserResolver(attribute.ServiceType, attribute.ImplementationType, parserResolverLifetime);
            }
        }

        protected override void ResolveMessageSubscription(Type invokerType, ServiceLifetime? lifetime)
        {
            foreach (RegisterBilibiliMessageSubscriptionAttribute attribute in invokerType.GetCustomAttributes<RegisterBilibiliMessageSubscriptionAttribute>(false))
            {
                TryAddService(attribute.ServiceType, attribute.ImplementationType, lifetime ?? attribute.Lifetime ?? DefaultSubscriptionLifetime, out _);
            }
        }

        protected override void ResolveMessageSubscriptionResolver(Type invokerType, ServiceLifetime? lifetime)
        {
            foreach (RegisterBilibiliMessageSubscriptionResolverAttribute attribute in invokerType.GetCustomAttributes<RegisterBilibiliMessageSubscriptionResolverAttribute>(false))
            {
                TryAddService(attribute.ServiceType, attribute.ImplementationType, lifetime ?? attribute.Lifetime ?? DefaultSubscriptionResolverLifetime, out _);
            }
        }
    }

    public static class BilibiliDanmakuFrameworkServiceCollectionExtensions
    {
        public static BilibiliDanmakuFrameworkBuilder AddBilibiliDanmakuFramework(this IServiceCollection services)
        {
            BilibiliDanmakuFrameworkBuilder builder = new BilibiliDanmakuFrameworkBuilder(services);
            builder.AddDanmakuOptions()
                   .AddParser<UnknownMessageParser>();
            return builder;
        }
    }
}
