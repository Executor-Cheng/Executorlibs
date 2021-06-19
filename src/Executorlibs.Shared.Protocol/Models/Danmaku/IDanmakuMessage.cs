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

    public class DanmakuMessage : DanmakuBaseMessage, IDanmakuMessage
    {
        public ITitle? Title { get; set; }

        public IMedal? Medal { get; set; }
    }

    /// <summary>
    /// 全平台通用的弹幕信息接口
    /// </summary>
    /// <typeparam name="TRawdata">原始数据类型</typeparam>
    /// <typeparam name="TUserId">用户Id的类型</typeparam>
    public interface IDanmakuMessage<TUserId, TRawdata> : IDanmakuMessage, IDanmakuBaseMessage<TUserId, TRawdata>
    {
#if !NETSTANDARD2_0
        IMedal? IDanmakuMessage.Medal => Medal;
#endif

        /// <summary>
        /// 勋章信息
        /// </summary>
        new IMedal<TUserId>? Medal { get; }
    }

    public class DanmakuMessage<TUserId, TRawdata> : DanmakuMessage, IDanmakuMessage<TUserId, TRawdata>
    {
        public new IMedal<TUserId>? Medal { get; set; }

        public TRawdata Rawdata { get; set; } = default!;

        public new TUserId UserId { get; set; } = default!;

        IMedal? IDanmakuMessage.Medal => Medal;
    }
}
