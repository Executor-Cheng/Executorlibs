using System;
#if !NETSTANDARD2_0
using Executorlibs.MessageFramework.Models.General;
#endif

namespace Executorlibs.Shared.Protocol.Models.General
{
    public interface IConnectionChangedMessage : IProtocolMessage
    {
#if !NETSTANDARD2_0
        int IProtocolMessage.RoomId => throw new NotSupportedException();

        long IMessage.Id => throw new NotSupportedException();
#endif
    }

    public class ConnectionChangedMessage : IConnectionChangedMessage
    {
        public long Id => throw new NotSupportedException();

        public int RoomId => throw new NotSupportedException();

        public DateTime Time { get; set; }
    }
}
