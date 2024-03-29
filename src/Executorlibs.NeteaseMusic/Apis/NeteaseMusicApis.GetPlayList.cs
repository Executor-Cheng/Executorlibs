using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Executorlibs.NeteaseMusic.Exceptions;
using Executorlibs.NeteaseMusic.Models;
using Executorlibs.Shared.Exceptions;
using Executorlibs.Shared.Extensions;

namespace Executorlibs.NeteaseMusic.Apis
{
    public static partial class NeteaseMusicApis
    {
        /// <summary>
        /// 获取歌单内的所有单曲
        /// </summary>
        /// <exception cref="NoSuchPlaylistException"/>
        /// <exception cref="PlaylistAccessDeniedException"/>
        /// <exception cref="UnknownResponseException"/>
        /// <param name="id">歌单Id</param>
        public static async Task<SongInfo[]> GetPlaylistAsync(HttpClient client, long id, CancellationToken token = default)
        {
            KeyValuePair<string?, string?>[] payload = new KeyValuePair<string?, string?>[3]
            {
                new KeyValuePair<string?, string?>("id", id.ToString()),
                new KeyValuePair<string?, string?>("n", "100000"),
                new KeyValuePair<string?, string?>("s", "8")
            };
            using JsonDocument j = await client.PostAsync("https://music.163.com/api/v6/playlist/detail", new FormUrlEncodedContent(payload), token).GetJsonAsync(token).ConfigureAwait(false);
            JsonElement root = j.RootElement;
            int code = root.GetProperty("code").GetInt32();
            switch (code)
            {
                case 200:
                    {
                        long[] songIds = root.GetProperty("playlist").GetProperty("trackIds").EnumerateArray().Select(p => p.GetProperty("id").GetInt64()).ToArray();
                        return await GetSongDetails(client, songIds, token);
                    }
                case 401:
                    {
                        throw new PlaylistAccessDeniedException(id);
                    }
                case 404:
                    {
                        throw new NoSuchPlaylistException(id);
                    }
                default:
                    {
                        throw new UnknownResponseException(root);
                    }
            }
        }
    }
}
