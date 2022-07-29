using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Executorlibs.NeteaseMusic.Crypto;
using Executorlibs.NeteaseMusic.Models;
using Executorlibs.Shared.Exceptions;
using Executorlibs.Shared.Extensions;

namespace Executorlibs.NeteaseMusic.Apis
{
    public static partial class NeteaseMusicApis
    {
        /// <summary>
        /// 异步创建用于二维码登录的key
        /// </summary>
        /// <exception cref="UnknownResponseException"></exception>
        public static async Task<string> CreateUnikeyAsync(HttpClient client, CancellationToken token = default)
        {
            IDictionary<string, object> data = new Dictionary<string, object>(1)
            {
                ["type"] = 1
            };
            CryptoHelper.WebApiEncryptedData encrypted = CryptoHelper.WebApiEncrypt(data);
            using JsonDocument j = await client.PostAsync("https://music.163.com/weapi/login/qrcode/unikey", encrypted.GetContent(), token).GetJsonAsync(token).ConfigureAwait(false);
            JsonElement root = j.RootElement;
            if (root.GetProperty("code").GetInt32() == 200)
            {
                return root.GetProperty("unikey").GetString()!;
            }
            throw new UnknownResponseException(root);
        }

        /// <summary>
        /// 异步获取key的状态
        /// </summary>
        /// <exception cref="UnknownResponseException"></exception>
        public static async Task<UnikeyStatus> GetUnikeyStatusAsync(HttpClient client, string unikey, CancellationToken token = default)
        {
            IDictionary<string, object> data = new Dictionary<string, object>(2)
            {
                ["key"] = unikey,
                ["type"] = 1
            };
            CryptoHelper.WebApiEncryptedData encrypted = CryptoHelper.WebApiEncrypt(data);
            using JsonDocument j = await client.PostAsync("https://music.163.com/weapi/login/qrcode/client/login", encrypted.GetContent(), token).GetJsonAsync(token).ConfigureAwait(false);
            JsonElement root = j.RootElement;
            UnikeyStatus result = (UnikeyStatus)root.GetProperty("code").GetInt32();
            if (!Enum.IsDefined(typeof(UnikeyStatus), result))
            {
                throw new UnknownResponseException(root);
            }
            return result;
        }
    }
}
