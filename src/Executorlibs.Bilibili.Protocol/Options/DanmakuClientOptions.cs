using System;

namespace Executorlibs.Bilibili.Protocol.Options
{
    public class DanmakuClientOptions
    {
        public uint RoomId { get; set; }

        public TimeSpan HeartbeatInterval { get; set; }

        public DanmakuClientOptions()
        {

        }

        public DanmakuClientOptions(uint roomId, TimeSpan heartbeatInterval)
        {
            RoomId = roomId;
            HeartbeatInterval = heartbeatInterval;
        }
    }
}
