using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;

namespace Executorlibs.Bilibili.Protocol.Parsing.Parsers
{
    /// <summary>
    /// 处理普通弹幕的 <see cref="IBilibiliMappableJsonMessageParser{TMessage}"/>
    /// </summary>
    public sealed class DanmakuParser : DanmakuParser<IDanmakuMessage, DanmakuMessage>
    {

    }

    /// <summary>
    /// 处理普通弹幕的 <see cref="IBilibiliMappableJsonMessageParser{TMessage}"/>
    /// </summary>
    public class DanmakuParser<TMessage, TImpl> : DanmakuParserBase<TMessage, TImpl>,
                                                  IBilibiliMappableJsonMessageParser<TMessage> where TMessage : IDanmakuMessage
                                                                                               where TImpl : DanmakuMessage, TMessage, new()
    {
        public string Key => Command;

        public sealed override bool CanParse(JsonElement root)
        {
            return root.TryGetProperty("cmd", out JsonElement cmdNode) && cmdNode.GetString() == Key;
        }
    }
}
