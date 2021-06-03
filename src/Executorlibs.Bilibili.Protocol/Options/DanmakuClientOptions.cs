using System;

namespace Executorlibs.Bilibili.Protocol.Options
{
    public class DanmakuClientOptions
    {
        public int RoomId { get; set; }

        public TimeSpan HeartbeatInterval { get; set; }
    }
}
