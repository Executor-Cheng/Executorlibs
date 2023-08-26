using System;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Builders;
using Executorlibs.Bilibili.Protocol.Clients;
using Executorlibs.Bilibili.Protocol.Dispatchers;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;
using Executorlibs.Bilibili.Protocol.Parsing.Contexts;
using Executorlibs.Bilibili.Protocol.Parsing.Parsers;
using Executorlibs.MessageFramework.Builders;
using Executorlibs.MessageFramework.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.Bilibili.Protocol.Extensions
{
    public static class BilibiliDanmakuFrameworkServiceCollectionExtensions
    {
        public static ParsingServiceBuilder<IDanmakuClient, byte[]> AddBilibiliDanmakuFramework(this IServiceCollection services)
        {
            return services.AddMessageFramework<IDanmakuClient>().WithRawdata<byte[]>();
        }

        public static ParsingContextServiceBuilder<IDanmakuClient, byte[], IBilibiliRawParsingContext> WithDefaultParsingContext(this ParsingServiceBuilder<IDanmakuClient, byte[]> builder)
        {
            return builder.WithParsingContext<IBilibiliRawParsingContext>();
        }

        public static RawdataDispatcherServiceBuilder<IDanmakuClient, byte[], IBilibiliRawdataDispatcher> WithDefaultDispatcher(this ParsingServiceBuilder<IDanmakuClient, byte[]> builder)
        {
            return builder.WithDispatcher<IBilibiliRawdataDispatcher>();
        }
         
        public static ParsingContextServiceBuilder<IDanmakuClient, JsonElement, IBilibiliJsonParsingContext> WithDefaultParsingContext(this ParsingServiceBuilder<IDanmakuClient, JsonElement> builder)
        {
            return builder.WithParsingContext<IBilibiliJsonParsingContext>();
        }

        public static ParsingServiceBuilder<IDanmakuClient, byte[]> AddDefaultRawdataDispatcher(this ParsingServiceBuilder<IDanmakuClient, byte[]> builder, Action<ParsingContextServiceBuilder<IDanmakuClient, byte[], IBilibiliRawParsingContext>>? parsingBuilderAction)
        {
            if (parsingBuilderAction != null)
            {
                var parsingContextBuilder = builder.WithDefaultParsingContext();
                parsingBuilderAction.Invoke(parsingContextBuilder);
            }
            builder.WithDefaultDispatcher().AddDefaultDispatcher();
            return builder;
        }

        public static RawdataDispatcherServiceBuilder<IDanmakuClient, byte[], IBilibiliRawdataDispatcher> AddDefaultDispatcher(this RawdataDispatcherServiceBuilder<IDanmakuClient, byte[], IBilibiliRawdataDispatcher> builder)
        {
            builder.AddComponent<BilibiliRawdataDispatcher>();
            return builder;
        }

        public static DefaultParsingContextBuilder<IDanmakuClient, byte[]> AddDefaultContext(this ParsingContextServiceBuilder<IDanmakuClient, byte[], IBilibiliRawParsingContext> builder)
        {
            builder.AddComponent<BilibiliRawParsingContext>();
            return new DefaultParsingContextBuilder<IDanmakuClient, byte[]>(builder.Builder);
        }

        public static DefaultParsingContextBuilder<IDanmakuClient, JsonElement> AddJsonContext(this ParsingContextServiceBuilder<IDanmakuClient, byte[], IBilibiliRawParsingContext> builder)
        {
            builder.AddComponent<BilibiliRawToJsonParsingContext>();
            return new DefaultParsingContextBuilder<IDanmakuClient, JsonElement>(builder.Builder.WithRawdata<JsonElement>() );
            
        }

        public static DanmakuCredentialProviderBuilder WithDanmakuCredentialProvider(this ParsingServiceBuilder<IDanmakuClient, byte[]> builder)
        {
            return new DanmakuCredentialProviderBuilder(builder);
        }
    }
}
