using System.Diagnostics;
using Executorlibs.Bilibili.Protocol.Models.Enums;
using Executorlibs.Bilibili.Protocol.Parsing.Parsers;
using Executorlibs.Bilibili.Protocol.Parsing.Parsers.Attributes;

namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    /// <summary>
    /// 表示欢迎老爷进入房间消息的接口
    /// </summary>
    /// <remarks>
    /// 消息来源是 Bilibili 直播平台<para/>
    /// 继承自以下接口:
    /// <list type="number">
    /// <item><see cref="IUserMessage"/></item>
    /// <item><see cref="ILordMessage"/></item>
    /// <item><see cref="IAdminMessage"/></item>
    /// </list>
    /// </remarks>
    [RegisterBilibiliParser(typeof(WelcomeParser))]
    public interface IWelcomeMessage : IUserMessage, ILordMessage, IAdminMessage
    {

    }

    /// <summary>
    /// 表示欢迎老爷进入房间消息
    /// </summary>
    /// <remarks>
    /// 消息来源是 Bilibili 直播平台
    /// </remarks>
    [DebuggerDisplay("{Time:yyyy-MM-dd HH:mm:ss} [Welcome] {UserName,nq}[{UserId}]")]
    public class WelcomeMessage : UserMessage, IWelcomeMessage
    {
        /// <inheritdoc/>
        public LordType LordType { get; set; }

        /// <inheritdoc/>
        public bool IsAdmin { get; set; }
    }
}
