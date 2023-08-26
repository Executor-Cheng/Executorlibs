namespace Executorlibs.Shared.Protocol.Models.Danmaku
{
    /// <summary>
    /// 全平台通用的礼物基本信息接口
    /// </summary>
    public interface ISendGiftBaseMessage : IUserMessage
    {
        /// <summary>
        /// 礼物名称
        /// </summary>
        string GiftName { get; }
        /// <summary>
        /// 礼物数量
        /// </summary>
        uint GiftCount { get; }
        /// <summary>
        /// 礼物价格 (单位:元)
        /// </summary>
        double GiftPrice { get; }
        /// <summary>
        /// 是否为免费礼物
        /// </summary>
        bool IsFree { get; }
    }
}
