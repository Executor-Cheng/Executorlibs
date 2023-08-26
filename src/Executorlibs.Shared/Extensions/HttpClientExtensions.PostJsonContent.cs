#if NETSTANDARD2_1 || NETSTANDARD2_0
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Executorlibs.Shared.Extensions
{
    public static partial class HttpClientExtensions
    {
        /// <inheritdoc cref="PostAsJsonAsync{TValue}(HttpClient, Uri, TValue, JsonSerializerOptions?, CancellationToken)"/>
        public static Task<HttpResponseMessage> PostAsJsonAsync<TValue>(this HttpClient client, Uri uri, TValue value, CancellationToken token = default)
        {
            return client.PostAsJsonAsync(uri, value, null, token);
        }

        /// <inheritdoc cref="PostAsJsonAsync{TValue}(HttpClient, string, TValue, JsonSerializerOptions?, CancellationToken)"/>
        public static Task<HttpResponseMessage> PostAsJsonAsync<TValue>(this HttpClient client, string url, TValue value, CancellationToken token = default)
        {
            return client.PostAsJsonAsync(url, value, null, token);
        }

        /// <param name="value">The value to be serialized to <see cref="HttpContent"/>.</param>
        /// <param name="options">A <see cref="JsonSerializerOptions"/> to be used while serializing the <paramref name="value"/> to <see cref="HttpContent"/>.</param>
        /// <inheritdoc cref="PostAsync(HttpClient, Uri, byte[], CancellationToken)"/>
        public static Task<HttpResponseMessage> PostAsJsonAsync<TValue>(this HttpClient client, Uri uri, TValue value, JsonSerializerOptions? options, CancellationToken token = default)
        {
            ByteArrayContent content = new ByteArrayContent(JsonSerializer.SerializeToUtf8Bytes(value, options));
            content.Headers.ContentType = DefaultJsonMediaType;
            return client.PostAsync(uri, content, token);
        }

        /// <param name="url">The url the request is sent to.</param>
        /// <inheritdoc cref="PostAsJsonAsync{TValue}(HttpClient, Uri, TValue, JsonSerializerOptions?, CancellationToken)"/>
        public static Task<HttpResponseMessage> PostAsJsonAsync<TValue>(this HttpClient client, string url, TValue value, JsonSerializerOptions? options, CancellationToken token = default)
           => client.PostAsJsonAsync(new Uri(url), value, options, token);
    }
}
#endif
