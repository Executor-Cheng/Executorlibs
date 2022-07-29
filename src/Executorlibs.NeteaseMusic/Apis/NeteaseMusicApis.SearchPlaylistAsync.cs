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
        public static async Task<PlaylistInfo[]> SearchPlaylistsAsync(HttpClient client, string keywords, int pageSize = 30, int offset = 0, CancellationToken token = default)
        {
            using JsonDocument j = await SearchAsync(client, keywords, SearchType.SongList, pageSize, offset, token).ConfigureAwait(false);
            JsonElement root = j.RootElement;
            if (root.GetProperty("code").GetInt32() == 200)
            {
                return root.GetProperty("result").GetProperty("playlists").EnumerateArray().Select(PlaylistInfo.Parse).ToArray();
            }
            throw new UnknownResponseException(root);
        }
    }
}
