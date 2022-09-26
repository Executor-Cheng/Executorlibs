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
        public static async Task<SongInfo> GetSongDetail(HttpClient client, long songId, CancellationToken token = default)
        {
            SongInfo[] songs = await GetSongDetails(client, new long[1] { songId }, token);
            if (songs.Length == 0)
            {
                throw new NoSuchSongException();
            }
            return songs[0];
        }

        public static async Task<SongInfo[]> GetSongDetails(HttpClient client, long[] songIds, CancellationToken token = default)
        {
            SongInfo[] result = new SongInfo[songIds.Length];
            for (int i = 0; i < songIds.Length; )
            {
                KeyValuePair<string?, string?>[] payload = new KeyValuePair<string?, string?>[1]
                {
                    new KeyValuePair<string?, string?>("c", JsonSerializer.Serialize(songIds.Skip(i).Take(Math.Min(1000, songIds.Length - i)).Select(p => new { id = p })))
                };
                using JsonDocument j = await client.PostAsync("https://music.163.com/api/v3/song/detail", new FormUrlEncodedContent(payload), token).GetJsonAsync(token);
                JsonElement root = j.RootElement;
                if (root.GetProperty("code").GetInt32() != 200)
                {
                    throw new UnknownResponseException(root);
                }
                foreach (JsonElement songNode in root.GetProperty("songs").EnumerateArray())
                {
                    result[i++] = SongInfo.Parse(songNode);
                }
            }
            return result;   
        }
    }
}
