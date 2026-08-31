using System;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Executorlibs.Bilibili.Protocol.Models;
using Executorlibs.Shared.Exceptions;
using Executorlibs.Shared.Extensions;
using Executorlibs.Shared.Helpers;

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
            using var j = await _client.GetAsync($"https://api.live.bilibili.com/xlive/web-room/v1/index/getDanmuInfo?id={roomId}&type=0", token).GetJsonAsync(token);
            var root = j.RootElement;
            if (root.GetProperty("code").GetInt32() == 0)
            {
                var data = root.GetProperty("data");
                var server = data.GetProperty("host_list");
                var guid = Guid.NewGuid();
                return new DanmakuServerInfo(server.EnumerateArray().Select(p => new DanmakuServerHostInfo(
                    p.GetProperty("host").GetString()!,
                    p.GetProperty("port").GetInt32(),
                    p.GetProperty("ws_port").GetInt32(),
                    p.GetProperty("wss_port").GetInt32()
                    )).ToArray(), userId, CreateBuvid(), data.GetProperty("token").GetString()!);
            }
            throw new UnknownResponseException(in root);
        }

        static unsafe string CreateBuvid()
        {
#if !NETSTANDARD2_0
            return string.Create(46, default(object), CreateInfoc);

#if !NETSTANDARD2_1
            [SkipLocalsInit]
#endif
            static void CreateInfoc(Span<char> span, object? o)
            {
                byte* ptr = stackalloc byte[21];
                var buffer = new Span<byte>(ptr, 21);
                RandomNumberGenerator.Fill(buffer);
                if (BitConverter.IsLittleEndian)
                {
                    *(uint*)(ptr + 17) &= 0x777777FF;
                }
                else
                {
                    *(uint*)(ptr + 17) &= 0xFF777777;
                }
                HexConverter.EncodeToUtf16(buffer, span, HexConverter.Casing.Upper);
                "infoc".AsSpan().CopyTo(span[41..46]);
                span[23] = span[18] = span[13] = span[8] = '-';
            }
#else
            var g = Guid.NewGuid();
            ulong* p = (ulong*)&g;
            uint r = (uint)(((*p) ^ (*++p)) % 10000);
            return string.Concat(g.ToString().ToUpperInvariant(), r.ToString(), "infoc");
#endif
        }
    }
}
