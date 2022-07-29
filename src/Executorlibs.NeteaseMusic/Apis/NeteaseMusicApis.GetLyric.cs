using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Executorlibs.NeteaseMusic.Models;
using Executorlibs.Shared.Exceptions;
using Executorlibs.Shared.Extensions;

namespace Executorlibs.NeteaseMusic.Apis
{
    public static partial class NeteaseMusicApis
    {
        /// <summary>
        /// 获取给定单曲ID的歌词
        /// </summary>
        /// <exception cref="UnknownResponseException"/>
        /// <param name="songId">单曲ID</param>
        public static async Task<LyricInfo?> GetLyricAsync(HttpClientv2 client, long songId, CancellationToken token = default)
        {
            KeyValuePair<string, string>[] payload = new KeyValuePair<string, string>[5]
            {
                new KeyValuePair<string, string>("id", songId.ToString()),
                new KeyValuePair<string, string>("lv", "-1"),
                new KeyValuePair<string, string>("tv", "-1"),
                new KeyValuePair<string, string>("rv", "-1"),
                new KeyValuePair<string, string>("kv", "-1")
            };
            using JsonDocument j = await client.PostAsync("https://music.163.com/api/song/lyric?_nmclfl=1", new FormUrlEncodedContent(payload), token).GetJsonAsync(token).ConfigureAwait(false);
            JsonElement root = j.RootElement;
            if (root.GetProperty("code").GetInt32() == 200)
            {
                string? lyricText = root.TryGetProperty("lrc", out JsonElement lrcElement) && lrcElement.TryGetProperty("lyric", out JsonElement lrcInnerElement) ? lrcInnerElement.GetString()! : null;
                string? translatedLyricText = root.TryGetProperty("tlyric", out lrcElement) && lrcElement.TryGetProperty("lyric", out lrcInnerElement) ? lrcInnerElement.GetString()! : null;
                LyricInfo? lyric = null;
                if (!string.IsNullOrEmpty(lyricText))
                {
                    lyric = new LyricInfo(lyricText);
                    if (!string.IsNullOrEmpty(translatedLyricText))
                    {
                        lyric.AppendLyric(translatedLyricText);
                    }
                }
                return lyric;
            }
            throw new UnknownResponseException(root);
        }
    }
}
