using System;
using Executorlibs.Bilibili.Protocol.Models.General;

namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    public interface IPkBaseMessage : IBilibiliJsonMessage
    {
        uint RedRoomId { get; }

        uint BlueRoomId { get; }

        DateTime EndTime { get; }
    }

    public interface IPkStartMessage : IPkBaseMessage
    {

    }

    public interface IPkEndMessage : IPkBaseMessage
    {
        ulong RedScore { get; }

        string RedBestUserName { get; }

        int RedMatchResult { get; }

        ulong BlueScore { get; }

        string BlueBestUserName { get; }

        int BlueMatchResult { get; }
    }

    public abstract class PkBaseMessage : BilibiliJsonMessage, IPkBaseMessage
    {
        public uint RedRoomId { get; set; }

        public uint BlueRoomId { get; set; }

        public DateTime EndTime { get; set; }
    }

    public class PkStartMessage : PkBaseMessage, IPkStartMessage
    {

    }

    public class PkEndMessage : PkBaseMessage, IPkEndMessage
    {
        public ulong RedScore { get; set; }

        public string RedBestUserName { get; set; } = null!;

        public int RedMatchResult { get; set; }

        public ulong BlueScore { get; set; }

        public string BlueBestUserName { get; set; } = null!;

        public int BlueMatchResult { get; set; }
    }
}
