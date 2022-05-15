using Executorlibs.Shared.Protocol.Clients;

namespace Executorlibs.Huya.Protocol.Clients
{
    public interface IHuyaClient : IProtocolClient
    {
        long RoomId { get; }
    }
}
