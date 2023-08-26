using System.Threading;
using System.Threading.Tasks;
using Executorlibs.Bilibili.Protocol.Options;

namespace Executorlibs.Bilibili.Protocol.Clients
{
    public readonly struct DanmakuClientContext : IDanmakuClient
    {
        private readonly IDanmakuClient _client;

        private readonly uint _roomId;

        public DanmakuClientContext(IDanmakuClient client, uint roomId)
        {
            _client = client;
            _roomId = roomId;
        }

        public uint RoomId => _roomId;

        public bool Connected => _client.Connected;

        public Task ConnectAsync(DanmakuClientOptions options, CancellationToken token = default)
        {
            return _client.ConnectAsync(options, token);
        }

        public void Disconnect()
        {
            _client.Disconnect();
        }
    }
}
