using System.Diagnostics;
using Executorlibs.Bilibili.Protocol.Parsing.Parsers.Attributes;

namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    /// <summary>
    /// 表示购买船员消息的接口
    /// </summary>
    /// <remarks>
    /// 消息来源是 Bilibili 直播平台<para/>
    /// 继承自 <see cref="ISendGiftBaseMessage"/>
    /// </remarks>
    [RegisterBilibiliParser(typeof(UserToastGuardBuyMessage))]
    public interface IUserToastGuardBuyMessage : IGuardBuyMessage
    {

    }

    /// <summary>
    /// 表示一条购买船员消息
    /// </summary>
    /// <remarks>
    /// 消息来源是 Bilibili 直播平台
    /// </remarks>
    [DebuggerDisplay("{Time.ToString(\"u\")[..^1],nq} [GuardBuy] {UserName,nq}[{UserId}]:{GiftName,nq}x{GiftCount}")]
    public class UserToastGuardBuyMessage : GuardBuyMessage, IUserToastGuardBuyMessage
    {
        
    }
}
