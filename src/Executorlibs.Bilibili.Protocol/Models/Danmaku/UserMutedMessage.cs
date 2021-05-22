using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Parsers;
using Executorlibs.Bilibili.Protocol.Parsers.Attributes;
using Executorlibs.Shared.Protocol.Models.Danmaku;

namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    /// <summary>
    /// 表示用户被禁言消息的接口
    /// </summary>
    /// <remarks>
    /// 消息来源是 Bilibili 直播平台<para/>
    /// 继承自以下接口:
    /// <list type="number">
    /// <item><see cref="IUserMutedMessage{TUserId, TRawdata}"/></item>
    /// <item><see cref="IUserMessage"/></item>
    /// </list>
    /// </remarks>
    [RegisterBilibiliParser(typeof(UserMutedParser))]
    public interface IUserMutedMessage : IUserMutedMessage<int, JsonElement>, IUserMessage
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
