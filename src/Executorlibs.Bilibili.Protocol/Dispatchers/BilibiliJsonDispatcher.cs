using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Executorlibs.Bilibili.Protocol.Clients;
using Executorlibs.Bilibili.Protocol.Models.General;
using Executorlibs.Bilibili.Protocol.Parsing.Contexts;
using Executorlibs.MessageFramework.Dispatchers;

namespace Executorlibs.Bilibili.Protocol.Dispatchers
{
    public interface IBilibiliJsonDispatcher : IRawdataDispatcher<IDanmakuClient, JsonElement>
    {

    }

    public class BilibiliJsonDispatcher : RawdataDispatcher<IDanmakuClient, JsonElement>, IBilibiliJsonDispatcher
    {
        protected readonly IDictionary<string, IBilibiliJsonParsingContext> _mappedParsingContexts;

        protected readonly IBilibiliJsonParsingContext[] _nonMappedContexts;

        protected readonly IBilibiliJsonParsingContext<IUnknownJsonMessage>? _unknownMessageParsingContext;

        public BilibiliJsonDispatcher(IEnumerable<IBilibiliJsonParsingContext> contexts)
        {
            var mappedParsingContexts = new Dictionary<string, IBilibiliJsonParsingContext>();
            var nonMappedContexts = new List<IBilibiliJsonParsingContext>();
            foreach (var context in contexts)
            {
                if (context is IBilibiliJsonParsingContext bpContext &&
                    bpContext.TryGetMessageKey(out string? key))
                {
                    if (mappedParsingContexts.ContainsKey(key!))
                    {
                        throw new InvalidOperationException("不应注册多个具有相同键值的解析上下文");
                    }
                    mappedParsingContexts.Add(key!, bpContext);
                    continue;
                }
                if (context is IBilibiliJsonParsingContext<IUnknownJsonMessage> unknownMessageParsingContext)
                {
                    if (_unknownMessageParsingContext != null)
                    {
                        throw new InvalidOperationException("不应注册多个对未知消息的解析上下文");
                    }
                    _unknownMessageParsingContext = unknownMessageParsingContext;
                    continue;
                }
                nonMappedContexts.Add(context);
            }
            _mappedParsingContexts = mappedParsingContexts;
            _nonMappedContexts = nonMappedContexts.ToArray();
        }

        public override Task HandleRawdataAsync(IDanmakuClient client, JsonElement rawdata)
        {
            if (rawdata.TryGetProperty("cmd", out JsonElement cmdToken) &&
                _mappedParsingContexts.TryGetValue(cmdToken.GetString()!, out var mappedContext))
            {
                return mappedContext.InvokeAsync(client, rawdata);
            }
            foreach (var context in _nonMappedContexts)
            {
                if (context.CanParse(rawdata))
                {
                    return context.InvokeAsync(client, rawdata);
                }
            }
            if (_unknownMessageParsingContext != null)
            {
                return _unknownMessageParsingContext.InvokeAsync(client, rawdata);
            }
            return Task.CompletedTask;
        }
    }
}
