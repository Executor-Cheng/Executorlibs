using Executorlibs.Bilibili.Protocol.Models.Danmaku;

namespace Executorlibs.Bilibili.Protocol.Parsing.Parsers
{
    /// <summary>
    /// 处理主播下播消息的 <see cref="IBilibiliJsonMessageParser{TMessage}"/>
    /// </summary>
    public class LiveEndParser : SimpleBilibiliMessageParser<ILiveEndMessage, LiveEndMessage>
    {
        private const string Command = "PREPARING";

        /// <inheritdoc/>
        public override string Key => Command;

        /// <summary>
        /// 初始化 <see cref="LiveEndParser"/> 类的新实例
        /// </summary>
        public LiveEndParser() { }
    }
}
