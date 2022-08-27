using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Executorlibs.NeteaseMusic.Crypto;
using Executorlibs.NeteaseMusic.Models;
using Executorlibs.Shared.Extensions;

namespace Executorlibs.NeteaseMusic.Apis
{
    public static partial class NeteaseMusicApis
    {
        /// <summary>
        /// 获取版权以及下载链接
        /// </summary>
        /// <param name="client"></param>
        /// <param name="songIds"></param>
        /// <param name="bitRate"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private static Task<JsonDocument> GetWebPlayerUrlResponseAsync(HttpClient client, long[] songIds, Quality bitRate = Quality.SuperQuality, CancellationToken token = default)
        {
            IDictionary<string, object> data = new Dictionary<string, object>
            {
                ["ids"] = songIds,
                ["br"] = (int)bitRate
            };
            CryptoHelper.WebApiEncryptedData encrypted = CryptoHelper.WebApiEncrypt(data);
            return client.PostAsync("https://music.163.com/weapi/song/enhance/player/url", encrypted.GetContent(), token).GetJsonAsync(token);
        }
    }
}
