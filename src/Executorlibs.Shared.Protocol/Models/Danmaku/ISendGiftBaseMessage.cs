namespace Executorlibs.Shared.Protocol.Models.Danmaku
{
    /// <summary>
    /// 全平台通用的礼物基本信息接口
    /// </summary>
    public interface ISendGiftBaseMessage : IUserMessage
    {
        /// <summary>
        /// 礼物Id
        /// </summary>
        int GiftId { get; } // 存疑, 不知道其它平台的Id是不是一定为Int32
        /// <summary>
        /// 礼物名称
        /// </summary>
        string GiftName { get; }
        /// <summary>
        /// 礼物数量
        /// </summary>
        int GiftCount { get; }
        /// <summary>
        /// 礼物价格 (单位:元)
        /// </summary>
        double GiftPrice { get; }
        /// <summary>
        /// 是否为免费礼物
        /// </summary>
        bool IsFree { get; }
    }

    /// <summary>
    /// 全平台通用的礼物基本信息接口
    /// </summary>
    public interface ISendGiftBaseMessage<TRawdata, TUserId> : ISendGiftBaseMessage, IUserMessage<TRawdata, TUserId>
    {

    }
}
