using Executorlibs.Bilibili.Protocol.Models.Danmaku;

namespace Executorlibs.Bilibili.Protocol.Parsers
{
    /// <summary>
    /// 处理主播开播消息的 <see cref="IBilibiliMessageParser{TMessage}"/>
    /// </summary>
    public class LiveStartParser : SimpleBilibiliMessageParser<ILiveStartMessage, LiveStartMessage>
    {
        private const string Command = "LIVE";

        /// <inheritdoc/>
        public override string Key => Command;

        /// <summary>
        /// 初始化 <see cref="LiveStartParser"/> 类的新实例
        /// </summary>
        public LiveStartParser() { }
    }
}
