using Executorlibs.Bilibili.Protocol.Models.Danmaku;

namespace Executorlibs.Bilibili.Protocol.Parsers
{
    /// <summary>
    /// 处理当前直播间被直播管理员关闭消息的 <see cref="IBilibiliMessageParser{TMessage}"/>
    /// </summary>
    public sealed class RoomLockParser : RoomLockParser<IRoomLockMessage, RoomLockMessage>
    {
        /// <summary>
        /// 初始化 <see cref="RoomLockParser"/> 类的新实例
        /// </summary>
        public RoomLockParser() { }
    }

    public class RoomLockParser<TMessage, TImpl> : LiveManagementParser<TMessage, TImpl> where TMessage : IRoomLockMessage
                                                                                         where TImpl : LiveManagementMessage, TMessage, new()
    {
        private const string Command = "ROOM_LOCK";

        /// <inheritdoc/>
        public override string Key => Command;

        /// <summary>
        /// 初始化 <see cref="RoomLockParser{TMessage, TImpl}"/> 类的新实例
        /// </summary>
        public RoomLockParser() { }
    }
}
