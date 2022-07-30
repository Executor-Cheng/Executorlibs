namespace System.Net.Http
{
    public abstract class HttpClientv2 : HttpClient
    {
        private readonly HttpMessageHandler _handler;

        public CookieContainer Cookie
        {
            get => _handler switch
            {
                HttpClientHandler clientHandler => clientHandler.CookieContainer,
#if NETCOREAPP2_1_OR_GREATER
                SocketsHttpHandler socketsHandler => socketsHandler.CookieContainer,
#endif
                _ => throw new NotSupportedException()
            };
            set => _ = _handler switch
            {
                HttpClientHandler clientHandler => clientHandler.CookieContainer = value,
#if NETCOREAPP2_1_OR_GREATER
                SocketsHttpHandler socketsHandler => socketsHandler.CookieContainer = value,
#endif
                _ => throw new NotSupportedException()
            };
        }

        public HttpClientv2() : this(new HttpClientHandler())
        {

        }

        public HttpClientv2(HttpMessageHandler handler) : base(handler)
        {
            _handler = handler;
        }

        public void ClearCookie()
        {
            Cookie = new CookieContainer();
        }

        public void SetCookie(Uri uri, string cookie)
        {
            Cookie.SetCookies(uri, cookie);
        }
    }
}
