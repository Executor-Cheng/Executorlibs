using ISharedDanmakuBaseMessage = Executorlibs.Shared.Protocol.Models.Danmaku.IDanmakuMessage;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    /// <summary>
    /// 表示弹幕的基本信息接口
    /// </summary>
    /// <remarks>
    /// 继承自以下接口:
    /// <list type="number">
    /// <item><see cref="ISharedDanmakuBaseMessage"/></item>
    /// <item><see cref="IUserMessage"/></item>
    /// </list>
    /// </remarks>
    public interface IDanmakuBaseMessage : IUserMessage, ISharedDanmakuBaseMessage
    {
        /// <summary>
        /// 弹幕Token
        /// </summary>
        uint Token { get; }

        /// <summary>
        /// 用户等级
        /// </summary>
        uint Level { get; }
    }

    /// <summary>
    /// 实现弹幕的基本信息接口的抽象类
    /// </summary>
    public abstract class DanmakuBaseMessage : UserMessage, IDanmakuBaseMessage
    {
        /// <inheritdoc/>
        public uint Token { get; set; }

        /// <inheritdoc/>
        public string Comment { get; set; }

        /// <inheritdoc/>
        public uint Level { get; set; }
    }
}
