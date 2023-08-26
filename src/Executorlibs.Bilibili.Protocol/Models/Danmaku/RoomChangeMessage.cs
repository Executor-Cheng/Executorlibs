using Executorlibs.Bilibili.Protocol.Models.General;

namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    public interface IRoomChangeMessage : IBilibiliJsonMessage
    {
        uint AreaId { get; }

        uint SubAreaId { get; }

        string Title { get; }
    }

    public class RoomChangeMessage : BilibiliJsonMessage, IRoomChangeMessage
    {
        public uint AreaId { get; set; }

        public uint SubAreaId { get; set; }

        public string Title { get; set; } = null!;
    }
}
