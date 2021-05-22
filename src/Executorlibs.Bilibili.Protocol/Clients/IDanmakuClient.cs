using Executorlibs.MessageFramework.Clients;

namespace Executorlibs.Bilibili.Protocol.Clients
{
    public interface IDanmakuClient : IMessageClient
    {
        int RoomId { get; }
    }
}
