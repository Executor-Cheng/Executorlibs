using System;

namespace Executorlibs.Huya.Protocol.Options
{
    public class HuyaClientOptions
    {
        public long RoomId { get; set; }

        public TimeSpan HeartbeatInterval { get; set; }
    }
}
