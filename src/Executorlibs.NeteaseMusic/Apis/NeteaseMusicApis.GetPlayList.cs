﻿using ExtendNetease_DGJModule.Exceptions;
using ExtendNetease_DGJModule.Extensions;
using Executorlibs.NeteaseMusic.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

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
            KeyValuePair<string, string>[] payload = new KeyValuePair<string, string>[3]
            {
                new KeyValuePair<string, string>("id", id.ToString()),
                new KeyValuePair<string, string>("n", "100000"),
                new KeyValuePair<string, string>("s", "8")
            };
            JObject j = (JObject)await client.PostAsync("https://music.163.com/api/v6/playlist/detail", new FormUrlEncodedContent(payload), token).GetJsonAsync(token).ConfigureAwait(false);
            int code = j["code"].ToObject<int>();
            switch (code)
            {
                case 200:
                    {
                        SongInfo[] result = j["playlist"]["tracks"].Select(p => new SongInfo(p)).ToArray();
                        IDictionary<long, bool> canPlayDic = await CheckMusicStatusAsync(client, result.Select(p => p.Id).ToArray(), token);
                        foreach (SongInfo song in result)
                        {
                            if (canPlayDic.TryGetValue(song.Id, out bool canPlay))
                            {
                                song.CanPlay = canPlay;
                            }
                        }
                        return result;
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
                        throw new UnknownResponseException(j);
                    }
            }
        }
    }
}
