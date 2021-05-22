using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Executorlibs.Bilibili.Protocol.Models;
using Executorlibs.Bilibili.Protocol.Options;
using Executorlibs.Shared.Exceptions;
using Executorlibs.Shared.Extensions;
using Executorlibs.Shared.Net.Http;
using Microsoft.Extensions.Options;

namespace Executorlibs.Bilibili.Protocol.Services
{
    public interface IDanmakuServerProvider : IDisposable
    {
        Task<DanmakuServerInfo> GetDanmakuServerInfoAsync(CancellationToken token = default);
    }

    public class DanmakuServerProvider : IDanmakuServerProvider
    {
        private readonly HttpClient _Client;

        private readonly DanmakuClientOptions _Options;

        public DanmakuServerProvider(IOptionsSnapshot<DanmakuClientOptions> options, PCHttpClient? client = null)
        {
            _Client = client ?? new PCHttpClient();
            _Options = options.Value;
        }

        public async Task<DanmakuServerInfo> GetDanmakuServerInfoAsync(CancellationToken token = default)
        {
            using JsonDocument j = await _Client.GetAsync($"https://api.live.bilibili.com/xlive/web-room/v1/index/getDanmuInfo?id={_Options.RoomId}&type=0", token).GetJsonAsync(token);
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
                    )).ToArray(), data.GetProperty("token").GetString()!);
            }
            throw new UnknownResponseException(in root);
        }

        public void Dispose()
        {
            _Client.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
