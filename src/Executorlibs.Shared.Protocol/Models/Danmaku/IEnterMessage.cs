namespace Executorlibs.Shared.Protocol.Models.Danmaku
{
    /// <summary>
    /// 全平台通用的用户进入直播间消息接口
    /// </summary>
    public interface IEnterMessage : IUserMessage
    {

    }

    /// <summary>
    /// 全平台通用的用户进入直播间消息接口
    /// </summary>
    public interface IEnterMessage<TRawdata, TUserId> : IEnterMessage, IUserMessage<TRawdata, TUserId>
    {

    }
}
