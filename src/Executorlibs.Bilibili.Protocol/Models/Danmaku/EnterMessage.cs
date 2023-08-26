using Executorlibs.Bilibili.Protocol.Models.Enums;
using Executorlibs.Bilibili.Protocol.Parsing.Parsers;
using Executorlibs.Bilibili.Protocol.Parsing.Parsers.Attributes;
using ISharedEnterMessage = Executorlibs.Shared.Protocol.Models.Danmaku.IEnterMessage;

namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    /// <summary>
    /// 表示用户进入直播间的接口
    /// </summary>
    /// <remarks>
    /// 消息来源是 Bilibili 直播平台<para/>
    /// 继承自以下接口:
    /// <list type="number">
    /// <item><see cref="ISharedEnterMessage"/></item>
    /// <item><see cref="IInteractMessage"/></item>
    /// <item><see cref="ILordMessage"/></item>
    /// <item><see cref="IAdminMessage"/></item>
    /// </list>
    /// </remarks>
    [RegisterBilibiliParser(typeof(EnterParser))]
    public interface IEnterMessage : ISharedEnterMessage, IInteractMessage, ILordMessage, IAdminMessage
    {

    }

    /// <summary>
    /// 表示一条用户进入房间的消息
    /// </summary>
    public class EnterMessage : InteractMessage, IEnterMessage
    {
        /// <inheritdoc/>
        public LordType LordType
        {
            get => UserType switch
            {
                InteractUserType.SVip => LordType.Yearly,
                InteractUserType.Vip => LordType.Monthly,
                _ => LordType.None
            };
            set => UserType = value switch
            {
                LordType.Yearly => InteractUserType.SVip,
                LordType.Monthly => InteractUserType.Vip,
                _ => InteractUserType.Normal
            };
        }

        /// <inheritdoc/>
        public bool IsAdmin => (UserType & InteractUserType.Manager) != 0;
    }
}
