using System;
using Executorlibs.Shared.Protocol.Clients;

namespace Executorlibs.Bilibili.Protocol.Clients
{
    public interface IDanmakuClient : IProtocolClient, IDisposable
    {
        int RoomId { get; }
    }
}
