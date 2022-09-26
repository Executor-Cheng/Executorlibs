using System.Text.Json;
using Executorlibs.Shared.Protocol.Models.Danmaku;

namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    /// <summary>
    /// 表示弹幕的基本信息接口
    /// </summary>
    /// <remarks>
    /// 继承自以下接口:
    /// <list type="number">
    /// <item><see cref="IDanmakuBaseMessage{TRawdata, TUserId}"/></item>
    /// <item><see cref="IUserMessage"/></item>
    /// </list>
    /// </remarks>
    public interface IDanmakuBaseMessage : IDanmakuBaseMessage<JsonElement, long>, IUserMessage
    {
        /// <summary>
        /// 弹幕Token
        /// </summary>
        int Token { get; }
        /// <summary>
        /// 用户等级
        /// </summary>
        int Level { get; }
    }

    /// <summary>
    /// 实现弹幕的基本信息接口的抽象类
    /// </summary>
    public abstract class DanmakuBaseMessage : UserMessage, IDanmakuBaseMessage
    {
        ///// <inheritdoc/>
        //public int Id { get; set; }

        /// <inheritdoc/>
        public virtual int Token { get; set; }

        /// <inheritdoc/>
        public string Comment { get; set; } = null!;

        /// <inheritdoc/>
        public int Level { get; set; }
    }
}
