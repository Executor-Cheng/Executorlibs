using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.General;
using Executorlibs.Shared.Protocol.Models.Danmaku;
#if NETSTANDARD2_0
using ISharedUserMessage = Executorlibs.Shared.Protocol.Models.Danmaku.IUserMessage;
#endif

namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    /// <summary>
    /// 表示具有用户信息的接口
    /// </summary>
    /// <remarks>
    /// 消息来源是 Bilibili 直播平台<para/>
    /// 继承自以下接口:
    /// <list type="number">
    /// <item><see cref="IUserMessage{TRawdata, TUserId}"/></item>
    /// <item><see cref="IBilibiliMessage"/></item>
    /// </list>
    /// </remarks>
    public interface IUserMessage : IUserMessage<JsonElement, long>, IBilibiliMessage
    {

    }

    /// <summary>
    /// 实现 <see cref="IUserMessage"/> 的抽象类
    /// </summary>
    /// <remarks>
    /// 消息来源是 Bilibili 直播平台
    /// </remarks>
    public abstract class UserMessage : BilibiliMessage, IUserMessage
    {
        /// <inheritdoc/>
        public string UserName { get; set; } = null!;

        /// <inheritdoc/>
        public long UserId { get; set; }

#if NETSTANDARD2_0
        object ISharedUserMessage.UserId => UserId!;
#endif
    }
}
