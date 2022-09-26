using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Executorlibs.NeteaseMusic.Crypto;
using Executorlibs.Shared.Exceptions;
using Executorlibs.Shared.Extensions;

namespace Executorlibs.NeteaseMusic.Apis
{
    public static partial class NeteaseMusicApis
    {
        public static async Task LogoutAsync(HttpClientv2 client, CancellationToken token = default)
        {
            IDictionary<string, object> data = new Dictionary<string, object>
            {
                ["csrf_token"] = GetCsrfToken(client),
            };
            CryptoHelper.WebApiEncryptedData encrypted = CryptoHelper.WebApiEncrypt(data);
            using JsonDocument j = await client.PostAsync("https://music.163.com/weapi/logout", encrypted.GetContent(), token).GetJsonAsync(token).ConfigureAwait(false);
            JsonElement root = j.RootElement;
            if (root.GetProperty("code").GetInt32() != 0)
            {
                throw new UnknownResponseException(root);
            }
        }
    }
}
