#if !NETSTANDARD2_0
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Executorlibs.Bilibili.Protocol.Dispatchers;
using Executorlibs.Bilibili.Protocol.Models.General;
using Executorlibs.Bilibili.Protocol.Services;

namespace Executorlibs.Bilibili.Protocol.Clients
{
    public class TcpDanmakuClientV3 : TcpDanmakuClient<BrotliPayloadDecoder>
    {
        public TcpDanmakuClientV3(IBilibiliRawdataDispatcher invoker, IDanmakuServerProvider credentialProvider, IBilibiliMessageDispatcher<IDisconnectedMessage>? disconnectDispatcher = null) : base(invoker, credentialProvider, disconnectDispatcher)
        {

        }

        protected override ValueTask SendJoinRoomAsync(Socket socket, uint roomId, ulong userId, string token, CancellationToken cToken = default)
        {
            return SendAsync(CreateJoinRoomPayload(3, roomId, userId, token), cToken);
        }
    }
}
#endif
