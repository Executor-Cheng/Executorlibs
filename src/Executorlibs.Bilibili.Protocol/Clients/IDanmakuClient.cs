using Executorlibs.Shared.Protocol.Clients;

namespace Executorlibs.Bilibili.Protocol.Clients
{
    public interface IDanmakuClient : IProtocolClient
    {
        int RoomId { get; }
    }
}
