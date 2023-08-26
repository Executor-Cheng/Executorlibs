using System;
using Executorlibs.Bilibili.Protocol.Options;
using IGeneralDisconnectedMessage = Executorlibs.Shared.Protocol.Models.General.IDisconnectedMessage;

namespace Executorlibs.Bilibili.Protocol.Models.General
{
    public interface IDisconnectedMessage : IBilibiliMessage, IGeneralDisconnectedMessage
    {
        DanmakuClientOptions ClientOptions { get; }
    }

    public class DisconnectedMessage : BilibiliMessage, IDisconnectedMessage
    {
        public DanmakuClientOptions ClientOptions { get; }

        public Exception? Exception { get; set; }

        public DisconnectedMessage(DanmakuClientOptions clientOptions) : this(clientOptions, null)
        {
           
        }

        public DisconnectedMessage(DanmakuClientOptions clientOptions, Exception? exception)
        {
            ClientOptions = clientOptions;
            Exception = exception;
        }
    }
}
