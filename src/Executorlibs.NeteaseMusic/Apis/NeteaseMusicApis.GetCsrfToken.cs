using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using Executorlibs.Shared.Exceptions;

namespace Executorlibs.NeteaseMusic.Apis
{
    public static partial class NeteaseMusicApis
    {
        private static string GetCsrfToken(HttpClientv2 client)
        {
            Cookie? cookie = client.Cookie.GetCookies(new Uri("http://music.163.com/")).OfType<Cookie>().FirstOrDefault(p => p.Name == "__csrf");
            if (cookie == null)
            {
                throw new InvalidCookieException();
            }
            return cookie.Value;
        }
    }
}
