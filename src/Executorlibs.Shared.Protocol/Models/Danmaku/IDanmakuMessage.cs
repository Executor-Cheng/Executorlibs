namespace Executorlibs.Shared.Protocol.Models.Danmaku
{
    /// <summary>
    /// 全平台通用的弹幕信息接口
    /// </summary>
    public interface IDanmakuMessage : IDanmakuBaseMessage
    {
        /// <summary>
        /// 头衔信息
        /// </summary>
        ITitle? Title { get; }
        /// <summary>
        /// 勋章信息
        /// </summary>
        IMedal? Medal { get; }
    }

    /// <summary>
    /// 全平台通用的弹幕信息接口
    /// </summary>
    /// <typeparam name="TRawdata">原始数据类型</typeparam>
    /// <typeparam name="TUserId">用户Id的类型</typeparam>
    public interface IDanmakuMessage<TUserId, TRawdata> : IDanmakuMessage, IDanmakuBaseMessage<TUserId, TRawdata>
    {
        IMedal? IDanmakuMessage.Medal => Medal;
        /// <summary>
        /// 勋章信息
        /// </summary>
        new IMedal<TUserId>? Medal { get; }
    }
}
