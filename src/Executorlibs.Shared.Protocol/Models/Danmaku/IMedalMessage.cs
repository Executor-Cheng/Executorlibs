namespace Executorlibs.Shared.Protocol.Models.Danmaku
{
    /// <summary>
    /// 全平台通用的勋章消息接口
    /// </summary>
    /// <typeparam name="TMedal"></typeparam>
    public interface IMedalMessage<TMedal> where TMedal : IMedal
    {
        TMedal? Medal { get; }
    }
}
