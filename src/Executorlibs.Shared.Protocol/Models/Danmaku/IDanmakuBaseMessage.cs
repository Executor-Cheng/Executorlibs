namespace Executorlibs.Shared.Protocol.Models.Danmaku
{
    /// <summary>
    /// 全平台通用的弹幕基本信息接口
    /// </summary>
    public interface IDanmakuBaseMessage : IUserMessage
    {
        /// <summary>
        /// 弹幕内容
        /// </summary>
        string Comment { get; }
    }

    public abstract class DanmakuBaseMessage : UserMessage, IDanmakuBaseMessage
    {
        public string Comment { get; set; } = null!;
    }

    /// <summary>
    /// 全平台通用的弹幕基本信息接口
    /// </summary>
    /// <typeparam name="TRawdata">原始数据类型</typeparam>
    /// <typeparam name="TUserId">用户Id的类型</typeparam>
    public interface IDanmakuBaseMessage<TUserId, TRawdata> : IDanmakuBaseMessage, IUserMessage<TUserId, TRawdata>
    {

    }

    public abstract class DanmakuBaseMessage<TUserId, TRawdata> : DanmakuBaseMessage, IDanmakuBaseMessage<TUserId, TRawdata>
    {
        public TRawdata Rawdata { get; set; } = default!;

        public new TUserId UserId { get; set; } = default!;
    }
}
