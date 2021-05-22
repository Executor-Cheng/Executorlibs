using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Executorlibs.Shared.Extensions
{
    public static partial class HttpClientExtensions
    {
        public static async Task<Stream> GetStreamAsync(this Task<HttpResponseMessage> responseTask, CancellationToken token = default)
        {
            HttpResponseMessage response = await responseTask.ConfigureAwait(false);
            HttpContent? c = response.Content;
#if NET5_0_OR_GREATER
            return c != null ? await c.ReadAsStreamAsync(token) : Stream.Null;
#else
            return c != null ? await c.ReadAsStreamAsync() : Stream.Null;
#endif
        }
    }
}
