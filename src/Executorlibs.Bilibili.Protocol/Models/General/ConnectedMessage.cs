using Executorlibs.Shared.Protocol.Models.General;
using IGeneralConnectedMessage = Executorlibs.Shared.Protocol.Models.General.IConnectedMessage;

namespace Executorlibs.Bilibili.Protocol.Models.General
{
    public interface IConnectedMessage : IBilibiliMessage, IGeneralConnectedMessage
    {

    }

    public class ConnectedMessage : BilibiliMessage, IConnectedMessage
    {
        public ConnectReason Reason { get; set; }
    }
}
