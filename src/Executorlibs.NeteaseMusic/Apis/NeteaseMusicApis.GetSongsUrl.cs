using Executorlibs.NeteaseMusic.Models;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Executorlibs.NeteaseMusic.Apis
{
    public static partial class NeteaseMusicApis
    {
        /// <summary>
        /// 批量获取单曲下载链接
        /// </summary>
        /// <param name="bitRate">比特率上限</param>
        /// <param name="songIds">单曲IDs</param>
        public static async Task<DownloadSongInfo[]> GetSongsUrlAsync(HttpClient client, long[] songIds, Quality bitRate = Quality.SuperQuality, CancellationToken token = default)
        {
            JsonDocument j = await GetPlayerUrlResponseAsync(client, songIds, bitRate, token).ConfigureAwait(false);
            JsonElement root = j.RootElement;
            return root.GetProperty("data").EnumerateArray().Select(p => new DownloadSongInfo(p.GetProperty("id").GetInt64(), p.GetProperty("br").GetInt32(), bitRate, p.GetProperty("url").GetString()!, p.GetProperty("type").GetString()!).ToArray();
        }
    }
}
