using Executorlibs.Bilibili.Protocol.Models.General;

namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    public interface IRoomChangeMessage : IBilibiliMessage
    {
        int AreaId { get; }

        int SubAreaId { get; }

        string Title { get; }
    }

    public class RoomChangeMessage : BilibiliMessage, IRoomChangeMessage
    {
        public int AreaId { get; set; }

        public int SubAreaId { get; set; }

        public string Title { get; set; } = null!;
    }
}
