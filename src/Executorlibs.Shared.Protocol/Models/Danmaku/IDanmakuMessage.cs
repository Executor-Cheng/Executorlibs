namespace Executorlibs.Shared.Protocol.Models.Danmaku
{
    /// <summary>
    /// 全平台通用的弹幕基本信息接口
    /// </summary>
    public interface IDanmakuMessage : IUserMessage
    {
        /// <summary>
        /// 弹幕内容
        /// </summary>
        string Comment { get; }
    }
}
