using System.Diagnostics;

namespace Executorlibs.Bilibili.Protocol.Models
{
    public class DanmakuServerInfo
    {
        public DanmakuServerHostInfo[] Hosts { get; }

        public ulong UserId { get; }

        public string Buvid { get; }

        public string Token { get; }

        public DanmakuServerInfo(DanmakuServerHostInfo[] hosts, ulong userId, string buvid, string token)
        {
            Hosts = hosts;
            UserId = userId;
            Buvid = buvid;
            Token = token;
        }
    }

    [DebuggerDisplay("{Host,nq}:{Port}/{WsPort}/{WssPort}")]
    public struct DanmakuServerHostInfo
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public int WsPort { get; set; }

        public int WssPort { get; set; }

        public DanmakuServerHostInfo(string host, int port, int wsPort, int wssPort)
        {
            Host = host;
            Port = port;
            WsPort = wsPort;
            WssPort = wssPort;
        }
    }
}
