namespace Executorlibs.Bilibili.Protocol.Models.General
{
    public interface IUnknownJsonMessage : IBilibiliJsonMessage
    {

    }

    public class UnknownJsonMessage : BilibiliJsonMessage, IUnknownJsonMessage
    {

    }
}
