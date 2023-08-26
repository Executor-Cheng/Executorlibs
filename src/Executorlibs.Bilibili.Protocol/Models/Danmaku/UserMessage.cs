using Executorlibs.Bilibili.Protocol.Models.General;
using ISharedUserMessage = Executorlibs.Shared.Protocol.Models.Danmaku.IUserMessage;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    /// <summary>
    /// 表示具有用户信息的接口
    /// </summary>
    /// <remarks>
    /// 消息来源是 Bilibili 直播平台<para/>
    /// 继承自以下接口:
    /// <list type="number">
    /// <item><see cref="ISharedUserMessage"/></item>
    /// <item><see cref="IBilibiliJsonMessage"/></item>
    /// </list>
    /// </remarks>
    public interface IUserMessage : ISharedUserMessage, IBilibiliJsonMessage
    {
        ulong UserId { get; }
    }

    /// <summary>
    /// 实现 <see cref="IUserMessage"/> 的抽象类
    /// </summary>
    /// <remarks>
    /// 消息来源是 Bilibili 直播平台
    /// </remarks>
    public abstract class UserMessage : BilibiliJsonMessage, IUserMessage
    {
        /// <inheritdoc/>
        public string UserName { get; set; }

        /// <inheritdoc/>
        public ulong UserId { get; set; }
    }
}
