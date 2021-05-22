using Executorlibs.Bilibili.Protocol.Models.Danmaku;

namespace Executorlibs.Bilibili.Protocol.Parsers
{
    /// <summary>
    /// 处理当前直播间被直播管理员切断消息的 <see cref="IBilibiliMessageParser{TMessage}"/>
    /// </summary>
    public sealed class CutOffParser : DefaultCutOffParser<ICutOffMessage, CutOffMessage>
    {
        /// <summary>
        /// 初始化 <see cref="CutOffParser"/> 类的新实例
        /// </summary>
        public CutOffParser() { }
    }

    public class DefaultCutOffParser<TMessage, TImpl> : DefaultLiveManagementParser<TMessage, TImpl> where TMessage : ICutOffMessage
                                                                                                     where TImpl : LiveManagementMessage, TMessage, new()
    {
        private const string Command = "CUT_OFF";

        /// <inheritdoc/>
        public override string Key => Command;

        /// <summary>
        /// 初始化 <see cref="DefaultCutOffParser{TMessage, TImpl}"/> 类的新实例
        /// </summary>
        public DefaultCutOffParser() { }
    }
}
