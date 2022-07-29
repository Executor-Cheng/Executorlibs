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
        /// 按给定的关键词搜索单曲
        /// </summary>
        /// <param name="keywords">关键词</param>
        /// <param name="pageSize">本次搜索返回的实例个数上限</param>
        /// <param name="offset">偏移量</param>
        public static async Task<SongInfo[]> SearchSongsAsync(HttpClient client, string keywords, int pageSize = 30, int offset = 0, CancellationToken token = default)
        {
            using JsonDocument j = await SearchAsync(client, keywords, SearchType.Song, pageSize, offset, token).ConfigureAwait(false);
            JsonElement root = j.RootElement;
            if (root.GetProperty("code").GetInt32() == 200)
            {
                SongInfo[] result = root.GetProperty("result").GetProperty("songs").EnumerateArray().Select(SongInfo.Parse).ToArray();
                IDictionary<long, bool> canPlayDic = await CheckMusicStatusAsync(client, result.Select(p => p.Id).ToArray(), Quality.SuperQuality, token);
                foreach (SongInfo song in result)
                {
                    if (canPlayDic.TryGetValue(song.Id, out bool canPlay))
                    {
                        song.CanPlay = canPlay;
                    }
                }
                return result;
            }
            throw new UnknownResponseException(root);
        }
    }
}
