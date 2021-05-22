namespace Executorlibs.Shared.Protocol.Models.General
{
    public enum ConnectReason
    {
        UserInitiated,

        PluginTriggered,

        ErrorEncountered,

        Others
    }

    public interface IConnectedMessage : IConnectionChangedMessage
    {
        ConnectReason Reason { get; }
    }

    public class ConnectedMessage : ConnectionChangedMessage, IConnectedMessage
    {
        public ConnectReason Reason { get; set; }
    }
}
