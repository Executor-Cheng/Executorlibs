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

    /// <summary>
    /// 全平台通用的弹幕基本信息接口
    /// </summary>
    /// <typeparam name="TRawdata">原始数据类型</typeparam>
    /// <typeparam name="TUserId">用户Id的类型</typeparam>
    public interface IDanmakuBaseMessage<TRawdata, TUserId> : IDanmakuBaseMessage, IUserMessage<TRawdata, TUserId>
    {

    }

    public abstract class DanmakuBaseMessage<TRawdata, TUserId> : UserMessage<TRawdata, TUserId>, IDanmakuBaseMessage<TRawdata, TUserId>
    {
        public string Comment { get; set; } = null!;
    }
}
