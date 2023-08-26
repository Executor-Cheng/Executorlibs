using Executorlibs.Bilibili.Protocol.Parsing.Parsers;
using Executorlibs.Bilibili.Protocol.Parsing.Parsers.Attributes;
using ISharedUserMutedMessage = Executorlibs.Shared.Protocol.Models.Danmaku.IUserMutedMessage;

namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    /// <summary>
    /// 表示用户被禁言消息的接口
    /// </summary>
    /// <remarks>
    /// 消息来源是 Bilibili 直播平台<para/>
    /// 继承自以下接口:
    /// <list type="number">
    /// <item><see cref="ISharedUserMutedMessage"/></item>
    /// <item><see cref="IUserMessage"/></item>
    /// </list>
    /// </remarks>
    [RegisterBilibiliParser(typeof(UserMutedParser))]
    public interface IUserMutedMessage : ISharedUserMutedMessage, IUserMessage
    {
        /// <summary>
        /// 是否为主播操作的禁言
        /// </summary>
        bool MasterOperation { get; }
    }

    /// <summary>
    /// 表示一条用户被禁言消息
    /// </summary>
    /// <remarks>
    /// 消息来源是 Bilibili 直播平台
    /// </remarks>
    public class UserMutedMessage : UserMessage, IUserMutedMessage
    {
        /// <inheritdoc/>
        public bool MasterOperation { get; set; }
    }
}
