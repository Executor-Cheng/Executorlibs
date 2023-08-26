using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Executorlibs.Bilibili.Protocol.Clients;
using Executorlibs.Bilibili.Protocol.Models.General;
using Executorlibs.Bilibili.Protocol.Parsing.Contexts;
using Executorlibs.Bilibili.Protocol.Utility;
using Executorlibs.MessageFramework.Dispatchers;

namespace Executorlibs.Bilibili.Protocol.Dispatchers
{
    public interface IBilibiliRawdataDispatcher : IRawdataDispatcher<IDanmakuClient, byte[]>
    {

    }

    public class BilibiliRawdataDispatcher : RawdataDispatcher<IDanmakuClient, byte[]>, IBilibiliRawdataDispatcher
    {
        protected readonly IDictionary<uint, IBilibiliRawParsingContext> _mappedParsingContexts;

        protected readonly IBilibiliRawParsingContext[] _nonMappedContexts;

        protected readonly IBilibiliRawParsingContext<IUnknownRawMessage>? _unknownMessageParsingContext;

        public BilibiliRawdataDispatcher(IEnumerable<IBilibiliRawParsingContext> contexts)
        {
            var mappedParsingContexts = new Dictionary<uint, IBilibiliRawParsingContext>();
            var nonMappedContexts = new List<IBilibiliRawParsingContext>();
            foreach (var context in contexts)
            {
                if (context is IBilibiliRawParsingContext bpContext &&
                    bpContext.TryGetMessageKey(out uint key))
                {
                    if (mappedParsingContexts.ContainsKey(key))
                    {
                        throw new InvalidOperationException("不应注册多个具有相同键值的解析上下文");
                    }
                    mappedParsingContexts.Add(key, bpContext);
                    continue;
                }
                if (context is IBilibiliRawParsingContext<IUnknownRawMessage> unknownMessageParsingContext)
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

        public override Task HandleRawdataAsync(IDanmakuClient client, byte[] rawdata)
        {
            ref var protocol = ref DanmakuProtocolUtility.AsProtocol(rawdata);
            if (_mappedParsingContexts.TryGetValue(protocol.Action, out var mappedContext))
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
