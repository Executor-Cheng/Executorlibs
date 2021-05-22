using System;
using Executorlibs.MessageFramework.Models.General;

namespace Executorlibs.Shared.Protocol.Models.General
{
    public interface IConnectionChangedMessage : IProtocolMessage
    {
        int IProtocolMessage.RoomId => throw new NotSupportedException();

        long IMessage.Id => throw new NotSupportedException();
    }

    public class ConnectionChangedMessage : IConnectionChangedMessage
    {
        public DateTime Time { get; set; }
    }
}
