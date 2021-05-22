using System;
using System.Threading;

namespace Executorlibs.Shared.Protocol.Models.General
{
    public interface IDisconnectedMessage : IConnectionChangedMessage
    {
        Exception? Exception { get; }

        CancellationToken Token { get; }
    }

    public class DisconnectedMessage : ConnectionChangedMessage, IDisconnectedMessage
    {
        public Exception? Exception { get; set; }

        public CancellationToken Token { get; set; }
    }
}
