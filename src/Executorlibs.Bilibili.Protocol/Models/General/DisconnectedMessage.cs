using System;
using System.Text.Json;
using GeneralDisconnectedMessage = Executorlibs.Shared.Protocol.Models.General.DisconnectedMessage;
using IGeneralDisconnectedMessage = Executorlibs.Shared.Protocol.Models.General.IDisconnectedMessage;

namespace Executorlibs.Bilibili.Protocol.Models.General
{
    public interface IDisconnectedMessage : IBilibiliMessage, IGeneralDisconnectedMessage
    {

    }

    public class DisconnectedMessage : GeneralDisconnectedMessage, IDisconnectedMessage
    {
        public JsonElement Rawdata => throw new NotImplementedException();
    }
}
