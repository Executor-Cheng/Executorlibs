using System.Net.Http;

namespace Executorlibs.Shared.Net.Http
{
    public class PCHttpClient : HttpClientv2
    {
        public PCHttpClient()
        {
            DefaultRequestHeaders.Accept.ParseAdd("*/*");
            DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36 Edg/87.0.664.66");
        }

        public PCHttpClient(HttpMessageHandler handler) : base(handler)
        {

        }
    }
}
