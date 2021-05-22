using System;
using Executorlibs.Bilibili.Protocol.Models.General;

namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    public interface IPkBaseMessage : IBilibiliMessage
    {
        int RedRoomId { get; }

        int BlueRoomId { get; }

        DateTime EndTime { get; }
    }

    public interface IPkStartMessage : IPkBaseMessage
    {
        
    }

    public interface IPkEndMessage : IPkBaseMessage
    {
        long RedScore { get; }

        string RedBestUserName { get; }

        int RedMatchResult { get; }

        long BlueScore { get; }

        string BlueBestUserName { get; }

        int BlueMatchResult { get; }
    }

    public abstract class PkBaseMessage : BilibiliMessage, IPkBaseMessage
    {
        public int RedRoomId { get; set; }

        public int BlueRoomId { get; set; }

        public DateTime EndTime { get; set; }
    }

    public class PkStartMessage : PkBaseMessage, IPkStartMessage
    {

    }

    public class PkEndMessage : PkBaseMessage, IPkEndMessage
    {
        public long RedScore { get; set; }

        public string RedBestUserName { get; set; } = null!;

        public int RedMatchResult { get; set; }

        public long BlueScore { get; set; }

        public string BlueBestUserName { get; set; } = null!;

        public int BlueMatchResult { get; set; }
    }
}
