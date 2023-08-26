using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Executorlibs.Bilibili.Protocol.Dispatchers;
using Executorlibs.Bilibili.Protocol.Models.General;
using Executorlibs.Bilibili.Protocol.Services;

namespace Executorlibs.Bilibili.Protocol.Clients
{
    public class WsDanmakuClientV2 : WsDanmakuClient<DeflatePayloadDecoder>
    {
        public WsDanmakuClientV2(IBilibiliRawdataDispatcher invoker, IDanmakuServerProvider credentialProvider, IBilibiliMessageDispatcher<IDisconnectedMessage>? disconnectDispatcher = null) : base(invoker, credentialProvider, disconnectDispatcher)
        {

        }

        protected override ValueTask SendJoinRoomAsync(WebSocket client, uint roomId, ulong userId, string token, CancellationToken cToken = default)
        {
            return SendAsync(new ReadOnlyMemory<byte>(CreateJoinRoomPayload(2, roomId, userId, token)), cToken);
        }
    }
}
