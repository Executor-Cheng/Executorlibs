using Executorlibs.MessageFramework.Models.General;

namespace Executorlibs.Shared.Protocol.Models.General
{
    public interface IConnectionChangedMessage : IMessage
    {

    }

    public class ConnectionChangedMessage : Message, IConnectionChangedMessage
    {

    }
}
