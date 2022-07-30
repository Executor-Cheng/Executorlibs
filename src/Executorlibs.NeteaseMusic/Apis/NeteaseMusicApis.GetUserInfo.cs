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
        public static async Task<UserInfo> GetUserInfoAsync(HttpClientv2 client, CancellationToken token = default)
        {
            using JsonDocument j = await client.GetAsync("https://music.163.com/api/nuser/account/get", token).GetJsonAsync(token).ConfigureAwait(false);
            JsonElement root = j.RootElement;
            if (root.GetProperty("code").GetInt32() == 0)
            {
                JsonElement profile = root.GetProperty("profile");
                if (profile.ValueKind == JsonValueKind.Null)
                {
                    throw new InvalidCookieException();
                }
                return new UserInfo(profile.GetProperty("nickname").GetString()!, profile.GetProperty("userId").GetInt64(), profile.GetProperty("vipType").GetInt32());
            }
            throw new UnknownResponseException(root);
        }
    }
}
