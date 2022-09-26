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
        /// 按给定的信息执行搜索, 并返回响应体
        /// </summary>
        /// <param name="keyWords">关键词</param>
        /// <param name="type">搜索类型</param>
        /// <param name="pageSize">返回的json中,实体个数上限</param>
        /// <param name="offset">偏移量</param>
        /// <returns>服务器返回的Json</returns>
        public static Task<JsonDocument> SearchAsync(HttpClient client, string keyWords, SearchType type, int pageSize = 30, int offset = 0, CancellationToken token = default)
        {
            IDictionary<string, object> data = new Dictionary<string, object>
            {
                ["s"] = keyWords,
                ["type"] = (int)type,
                ["limit"] = pageSize,
                ["offset"] = offset
            };
            CryptoHelper.WebApiEncryptedData encrypted = CryptoHelper.WebApiEncrypt(data);
            return client.PostAsync("https://music.163.com/weapi/cloudsearch/get/web", encrypted.GetContent(), token).GetJsonAsync(token);
        }
    }
}
