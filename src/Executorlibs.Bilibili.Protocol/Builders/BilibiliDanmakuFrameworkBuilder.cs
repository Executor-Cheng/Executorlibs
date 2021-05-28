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
        public BilibiliDanmakuFrameworkBuilder(IServiceCollection services) : base(services)
        {

        }

        public BilibiliDanmakuFrameworkBuilder AddCredentialProvider<TProvider>() where TProvider : class, IDanmakuServerProvider
        {
            Services.AddScoped<IDanmakuServerProvider, TProvider>();
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

        public override BilibiliDanmakuFrameworkBuilder AddHandler<THandler>()
        {
            return (BilibiliDanmakuFrameworkBuilder)base.AddHandler<THandler>();
        }

        public override BilibiliDanmakuFrameworkBuilder AddHandler<THandler>(ServiceLifetime lifetime)
        {
            return (BilibiliDanmakuFrameworkBuilder)base.AddHandler<THandler>(lifetime);
        }

        public override BilibiliDanmakuFrameworkBuilder AddInvoker<TInvoker>()
        {
            return (BilibiliDanmakuFrameworkBuilder)base.AddInvoker<TInvoker>();
        }

        public override BilibiliDanmakuFrameworkBuilder AddInvoker<TInvoker>(ServiceLifetime lifetime)
        {
            return (BilibiliDanmakuFrameworkBuilder)base.AddInvoker<TInvoker>(lifetime);
        }

        public override BilibiliDanmakuFrameworkBuilder AddParser<TParser>()
        {
            return (BilibiliDanmakuFrameworkBuilder)base.AddParser<TParser>();
        }

        public override BilibiliDanmakuFrameworkBuilder AddParser<TParser>(ServiceLifetime lifetime)
        {
            return (BilibiliDanmakuFrameworkBuilder)base.AddParser<TParser>(lifetime);
        }
#endif
        protected override void ResolveMessageParser(Type handlerType)
        {
            foreach (RegisterBilibiliParserAttribute attribute in handlerType.GetCustomAttributes<RegisterBilibiliParserAttribute>())
            {
                AddService(attribute.ServiceType, attribute.ImplementationType, attribute.Lifetime ?? DefaultParserLifetime);
            }
        }

        protected override void ResolveMessageSubscription(Type invokerType)
        {
            foreach (RegisterBilibiliMessageSubscriptionAttribute attribute in invokerType.GetCustomAttributes<RegisterBilibiliMessageSubscriptionAttribute>())
            {
                AddService(attribute.ServiceType, attribute.ImplementationType, attribute.Lifetime ?? DefaultSubscriptionLifetime);
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
