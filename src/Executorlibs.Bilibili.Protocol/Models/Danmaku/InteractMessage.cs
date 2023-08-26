using Executorlibs.Bilibili.Protocol.Models.Enums;
using Executorlibs.Bilibili.Protocol.Parsing.Parsers;
using Executorlibs.Bilibili.Protocol.Parsing.Parsers.Attributes;

namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    /// <summary>
    /// 表示用户互动消息的接口
    /// </summary>
    /// <remarks>
    /// 消息来源是 Bilibili 直播平台<para/>
    /// 继承自 <see cref="IUserMessage"/>
    /// </remarks>
    [RegisterBilibiliParser(typeof(InteractParser))]
    public interface IInteractMessage : IUserMessage
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        InteractType Type { get; }

        /// <summary>
        /// 用户类型
        /// </summary>
        InteractUserType UserType { get; }
    }

    /// <summary>
    /// 表示一条用户互动消息
    /// </summary>
    public class InteractMessage : UserMessage, IInteractMessage
    {
        /// <inheritdoc/>
        public InteractType Type { get; set; }

        /// <inheritdoc/>
        public InteractUserType UserType { get; set; }
    }
}
