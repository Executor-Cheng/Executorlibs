using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Executorlibs.Bilibili.Protocol.Models;
using Executorlibs.Shared.Exceptions;
using Executorlibs.Shared.Extensions;

namespace Executorlibs.Bilibili.Protocol.Services
{
    public interface IDanmakuServerProvider
    {
        Task<DanmakuServerInfo> GetDanmakuServerInfoAsync(uint roomId, CancellationToken token = default);
    }

    public class DanmakuServerProvider : IDanmakuServerProvider
    {
        protected readonly HttpClient _client;

        public DanmakuServerProvider(HttpClient client)
        {
            _client = client;
        }

        public virtual Task<DanmakuServerInfo> GetDanmakuServerInfoAsync(uint roomId, CancellationToken token = default)
        {
            return GetDanmakuServerInfoAsync(roomId, 0, token);
        }

        protected async Task<DanmakuServerInfo> GetDanmakuServerInfoAsync(uint roomId, ulong userId, CancellationToken token)
        {
            using JsonDocument j = await _client.GetAsync($"https://api.live.bilibili.com/xlive/web-room/v1/index/getDanmuInfo?id={roomId}&type=0", token).GetJsonAsync(token);
            JsonElement root = j.RootElement;
            if (root.GetProperty("code").GetInt32() == 0)
            {
                JsonElement data = root.GetProperty("data"),
                            server = data.GetProperty("host_list");
                return new DanmakuServerInfo(server.EnumerateArray().Select(p => new DanmakuServerHostInfo(
                    p.GetProperty("host").GetString()!,
                    p.GetProperty("port").GetInt32(),
                    p.GetProperty("ws_port").GetInt32(),
                    p.GetProperty("wss_port").GetInt32()
                    )).ToArray(), userId, data.GetProperty("token").GetString()!);
            }
            throw new UnknownResponseException(in root);
        }
    }
}
