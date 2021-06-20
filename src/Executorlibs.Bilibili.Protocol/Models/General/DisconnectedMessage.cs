using System;
using System.Threading;
using IGeneralDisconnectedMessage = Executorlibs.Shared.Protocol.Models.General.IDisconnectedMessage;

namespace Executorlibs.Bilibili.Protocol.Models.General
{
    public interface IDisconnectedMessage : IBilibiliMessage, IGeneralDisconnectedMessage
    {

    }

    public class DisconnectedMessage : BilibiliMessage, IDisconnectedMessage
    {
        public Exception? Exception { get; set; }

        public CancellationToken Token { get; set; }
    }
}
