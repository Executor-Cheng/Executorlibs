using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Executorlibs.Bilibili.Protocol.Dispatchers;
using Executorlibs.Bilibili.Protocol.Models.General;
using Executorlibs.Bilibili.Protocol.Services;
#if NETSTANDARD2_0
using System;
using Executorlibs.Shared.Extensions;
#endif

namespace Executorlibs.Bilibili.Protocol.Clients
{
    public class TcpDanmakuClientV2 : TcpDanmakuClient<DeflatePayloadDecoder>
    {
        public TcpDanmakuClientV2(IBilibiliRawdataDispatcher invoker, IDanmakuServerProvider credentialProvider, IBilibiliMessageDispatcher<IDisconnectedMessage>? disconnectDispatcher = null) : base(invoker, credentialProvider, disconnectDispatcher)
        {

        }

        protected override ValueTask SendJoinRoomAsync(Socket socket, uint roomId, ulong userId, string token, CancellationToken cToken = default)
        {
#if !NETSTANDARD2_0
            var task = socket.SendAsync(CreateJoinRoomPayload(2, roomId, userId, token), SocketFlags.None, cToken);
            if (task.IsCompletedSuccessfully)
            {
                task.GetAwaiter().GetResult();
                return default;
            }
            return new ValueTask(task.AsTask());
#else
            return new ValueTask(socket.SendAsync(new ArraySegment<byte>(CreateJoinRoomPayload(2, roomId, userId, token)), SocketFlags.None));
#endif
        }
    }
}
