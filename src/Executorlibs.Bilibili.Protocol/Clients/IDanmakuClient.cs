using Executorlibs.Bilibili.Protocol.Handlers;
using Executorlibs.Shared.Protocol.Clients;

namespace Executorlibs.Bilibili.Protocol.Clients
{
    public interface IDanmakuClient : IProtocolClient
    {
        int RoomId { get; }

        PluginResistration AddPlugin(IBilibiliMessageHandler handler);
    }
}
