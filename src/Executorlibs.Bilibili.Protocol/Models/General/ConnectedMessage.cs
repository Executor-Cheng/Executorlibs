using System;
using System.Text.Json;
using GeneralConnectedMessage = Executorlibs.Shared.Protocol.Models.General.ConnectedMessage;
using IGeneralConnectedMessage = Executorlibs.Shared.Protocol.Models.General.IConnectedMessage;

namespace Executorlibs.Bilibili.Protocol.Models.General
{
    public interface IConnectedMessage : IBilibiliMessage, IGeneralConnectedMessage
    {

    }

    public class ConnectedMessage : GeneralConnectedMessage, IConnectedMessage
    {
        public JsonElement Rawdata => throw new NotImplementedException();
    }
}
