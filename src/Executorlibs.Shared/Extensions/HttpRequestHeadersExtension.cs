using System.Net.Http.Headers;

namespace Executorlibs.Shared.Extensions
{
    public static class HttpRequestHeadersExtension
    {
        public static void SetUserAgent(this HttpRequestHeaders headers, string userAgent)
        {
            HttpHeaderValueCollection<ProductInfoHeaderValue> ua = headers.UserAgent;
            ua.Clear();
            ua.ParseAdd(userAgent);
        }

        public static void SetAccept(this HttpRequestHeaders headers, string accept)
        {
            HttpHeaderValueCollection<MediaTypeWithQualityHeaderValue> a = headers.Accept;
            a.Clear();
            a.ParseAdd(accept);
        }

        public static void SetAcceptLanguage(this HttpRequestHeaders headers, string acceptLanguage)
        {
            HttpHeaderValueCollection<StringWithQualityHeaderValue> al = headers.AcceptLanguage;
            al.Clear();
            al.ParseAdd(acceptLanguage);
        }

        public static void SetSecPolicy(this HttpRequestHeaders headers, string? mode = "cors", string? site = "same-site", string? dest = "empty")
        {
            if (!string.IsNullOrEmpty(mode))
            {
                headers.Add("Sec-Fetch-Mode", mode);
            }
            if (!string.IsNullOrEmpty(site))
            {
                headers.Add("Sec-Fetch-Site", site);
            }
            if (!string.IsNullOrEmpty(dest))
            {
                headers.Add("Sec-Fetch-Dest", dest);
            }
        }
    }
}
