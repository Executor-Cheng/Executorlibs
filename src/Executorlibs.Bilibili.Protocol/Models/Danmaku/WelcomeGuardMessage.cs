using System.Diagnostics;
using Executorlibs.Bilibili.Protocol.Parsers;
using Executorlibs.Bilibili.Protocol.Parsers.Attributes;
using Executorlibs.Bilibili.Protocol.Models.Enums;

namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    /// <summary>
    /// 表示欢迎船员进入房间消息的接口
    /// </summary>
    /// <remarks>
    /// 消息来源是 Bilibili 直播平台<para/>
    /// 继承自以下接口:
    /// <list type="number">
    /// <item><see cref="IUserMessage"/></item>
    /// <item><see cref="IGuardMessage"/></item>
    /// </list>
    /// </remarks>
    [RegisterBilibiliParser(typeof(WelcomeGuardParser))]
    public interface IWelcomeGuardMessage : IUserMessage, IGuardMessage
    {

    }

    /// <summary>
    /// 表示欢迎船员进入房间消息
    /// </summary>
    /// <remarks>
    /// 消息来源是 Bilibili 直播平台
    /// </remarks>
    [DebuggerDisplay("{Time.ToString(\"u\")[..^1],nq} [WelcomeGuard] {UserName,nq}[{UserId}]")]
    public class WelcomeGuardMessage : UserMessage, IWelcomeGuardMessage
    {
        /// <inheritdoc/>
        public GuardType GuardType { get; set; }
    }
}
