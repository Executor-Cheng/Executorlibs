using System;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
namespace Executorlibs.Bilibili.Protocol.Options
{
    public class DanmakuServerOptions
    {
        public uint RoomId { get; set; }

        public ulong UserId { get; set; }

        public string Token { get; set; }

        public TimeSpan JoinTimeout { get; set; }

        public TimeSpan HeartbeatInterval { get; set; }

        public DanmakuServerOptions()
        {

        }

        public DanmakuServerOptions(uint roomId, ulong userId, string token, TimeSpan joinTimeout, TimeSpan heartbeatInterval)
        {
            RoomId = roomId;
            UserId = userId;
            Token = token;
            JoinTimeout = joinTimeout;
            HeartbeatInterval = heartbeatInterval;
        }
    }
}
