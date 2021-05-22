using System.Reflection;

namespace System.Net.Http
{
    public abstract class HttpClientv2 : HttpClient
    {
        public readonly HttpClientHandler _Handler;

        public CookieContainer Cookie => _Handler.CookieContainer;

        public HttpClientv2()
        {
            _Handler = (HttpClientHandler)typeof(HttpMessageInvoker).GetField("_handler", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(this)!;
        }

        public HttpClientv2(HttpMessageHandler handler) : base(handler)
        {
            _Handler = (HttpClientHandler)handler;
        }

        public void ClearCookie()
        {
            _Handler.CookieContainer = new CookieContainer();
        }

        public void SetCookie(Uri uri, string cookie)
        {
            _Handler.CookieContainer.SetCookies(uri, cookie);
        }
    }
}
