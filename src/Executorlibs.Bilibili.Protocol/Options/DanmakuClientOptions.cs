using System;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
namespace Executorlibs.Bilibili.Protocol.Options
{
    public class DanmakuClientOptions
    {
        public uint RoomId { get; set; }

        public ushort Version { get; set; }

        public string Platform { get; set; }

        public TimeSpan HeartbeatInterval { get; set; }

        public DanmakuClientOptions()
        {

        }

        public DanmakuClientOptions(uint roomId, ushort version, string platform, TimeSpan heartbeatInterval)
        {
            RoomId = roomId;
            Version = version;
            Platform = platform;
            HeartbeatInterval = heartbeatInterval;
        }
    }
}
