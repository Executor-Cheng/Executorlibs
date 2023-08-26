using Executorlibs.Bilibili.Protocol.Models.Danmaku;

namespace Executorlibs.Bilibili.Protocol.Parsing.Parsers
{
    /// <summary>
    /// 处理当前直播间被直播管理员切断消息的 <see cref="IBilibiliJsonMessageParser{TMessage}"/>
    /// </summary>
    public sealed class CutOffParser : CutOffParser<ICutOffMessage, CutOffMessage>
    {
        /// <summary>
        /// 初始化 <see cref="CutOffParser"/> 类的新实例
        /// </summary>
        public CutOffParser() { }
    }

    public class CutOffParser<TMessage, TImpl> : LiveManagementParser<TMessage, TImpl> where TMessage : ICutOffMessage
                                                                                       where TImpl : LiveManagementMessage, TMessage, new()
    {
        private const string Command = "CUT_OFF";

        /// <inheritdoc/>
        public override string Key => Command;

        /// <summary>
        /// 初始化 <see cref="CutOffParser{TMessage, TImpl}"/> 类的新实例
        /// </summary>
        public CutOffParser() { }
    }
}
