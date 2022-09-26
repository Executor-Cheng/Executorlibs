using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Executorlibs.NeteaseMusic.Exceptions;
using Executorlibs.NeteaseMusic.Models;

namespace Executorlibs.NeteaseMusic.Apis
{
    public static partial class NeteaseMusicApis
    {
        /// <summary>
        /// 批量获取单曲下载链接
        /// </summary>
        /// <param name="bitRate">比特率上限</param>
        /// <param name="songId">单曲ID</param>
        public static async Task<DownloadSongInfo> GetSongsUrlAsync(HttpClient client, long songId, Quality bitRate = Quality.SuperQuality, CancellationToken token = default)
        {
            DownloadSongInfo[] songs = await GetSongsUrlAsync(client, new long[1] { songId }, bitRate, token);
            if (songs.Length == 0)
            {
                throw new NoSuchSongException();
            }
            return songs[0];
        }

        /// <summary>
        /// 批量获取单曲下载链接
        /// </summary>
        /// <param name="bitRate">比特率上限</param>
        /// <param name="songIds">单曲IDs</param>
        public static async Task<DownloadSongInfo[]> GetSongsUrlAsync(HttpClient client, long[] songIds, Quality bitRate = Quality.SuperQuality, CancellationToken token = default)
        {
            using JsonDocument j = await GetWebPlayerUrlResponseAsync(client, songIds, bitRate, token).ConfigureAwait(false);
            JsonElement root = j.RootElement;
            return root.GetProperty("data").EnumerateArray().Where(p => p.GetProperty("code").GetInt32() == 200).Select(DownloadSongInfo.Parse).ToArray();
        }
    }
}
