using System;
using Executorlibs.MessageFramework.Models.General;

namespace Executorlibs.Shared.Protocol.Models.General
{
    public interface IDisconnectedMessage : IMessage
    {
        Exception? Exception { get; }
    }
}
