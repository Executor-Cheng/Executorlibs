using Executorlibs.Bilibili.Protocol.Models.Danmaku;

namespace Executorlibs.Bilibili.Protocol.Parsing.Parsers
{
    /// <summary>
    /// 处理当前直播间被直播管理员警告消息的 <see cref="IBilibiliJsonMessageParser{TMessage}"/>
    /// </summary>
    public sealed class WarningParser : WarningParser<IWarningMessage, WarningMessage>
    {
        /// <summary>
        /// 初始化 <see cref="WarningParser"/> 类的新实例
        /// </summary>
        public WarningParser() { }
    }

    public class WarningParser<TMessage, TImpl> : LiveManagementParser<TMessage, TImpl> where TMessage : IWarningMessage
                                                                                        where TImpl : LiveManagementMessage, TMessage, new()
    {
        private const string Command = "WARNING";

        /// <inheritdoc/>
        public override string Key => Command;

        /// <summary>
        /// 初始化 <see cref="WarningParser{TMessage, TImpl}"/> 类的新实例
        /// </summary>
        public WarningParser() { }
    }
}
