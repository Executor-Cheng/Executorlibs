using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Executorlibs.NeteaseMusic.Models;
using Executorlibs.Shared.Exceptions;

namespace Executorlibs.NeteaseMusic.Apis
{
    public static partial class NeteaseMusicApis
    {
        /// <summary>
        /// 检查给定ID对应的音乐有无版权
        /// </summary>
        /// <param name="songIds">音乐ID</param>
        /// <returns></returns>
        public static async Task<IDictionary<long, bool>> CheckMusicStatusAsync(HttpClient client, long[] songIds, Quality quality, CancellationToken token = default)
        {
            using JsonDocument j = await GetPlayerUrlResponseAsync(client, songIds, Quality.SuperQuality, token).ConfigureAwait(false);
            JsonElement root = j.RootElement;
            if (root.GetProperty("code").GetInt32() == 200)
            {
                return root.GetProperty("data").EnumerateArray().ToDictionary(p => p.GetProperty("id").GetInt64(), p => p.GetProperty("code").GetInt32() == 200);
            }
            throw new UnknownResponseException(root);
        }
    }
}
