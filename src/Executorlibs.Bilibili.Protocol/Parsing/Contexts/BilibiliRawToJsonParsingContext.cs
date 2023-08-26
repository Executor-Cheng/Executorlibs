using System.Text.Json;
using System.Threading.Tasks;
using Executorlibs.Bilibili.Protocol.Clients;
using Executorlibs.Bilibili.Protocol.Dispatchers;
using Executorlibs.Bilibili.Protocol.Utility;

namespace Executorlibs.Bilibili.Protocol.Parsing.Contexts
{
    public class BilibiliRawToJsonParsingContext : BilibiliRawParsingContext
    {
        protected const uint BodyAction = 5;

        protected readonly IBilibiliJsonDispatcher _dispatcher;

        public BilibiliRawToJsonParsingContext(IBilibiliJsonDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public override bool TryGetMessageKey(out uint key)
        {
            key = BodyAction;
            return true;
        }

        public override bool CanParse(byte[] rawdata)
        {
            ref var protocol = ref DanmakuProtocolUtility.AsProtocol(rawdata);
            return protocol.Action == BodyAction;
        }

        public override Task InvokeAsync(IDanmakuClient client, byte[] rawdata)
        {
            return HandleRawdataAsync(client, JsonDocument.Parse(DanmakuProtocolUtility.GetBody(rawdata)));
        }

        private async Task HandleRawdataAsync(IDanmakuClient client, JsonDocument document)
        {
            try
            {
                await _dispatcher.HandleRawdataAsync(client, document.RootElement);
            }
            finally
            {
                document.Dispose();
            }
        }
    }
}
