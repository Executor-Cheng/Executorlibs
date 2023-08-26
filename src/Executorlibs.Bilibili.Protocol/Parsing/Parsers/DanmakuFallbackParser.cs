using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;

namespace Executorlibs.Bilibili.Protocol.Parsing.Parsers
{
    /// <summary>
    /// 处理特殊时期普通弹幕的 <see cref="IBilibiliJsonMessageParser{TMessage}"/>
    /// </summary>
    public sealed class DanmakuFallbackParser : DanmakuFallbackParser<IDanmakuMessage, DanmakuMessage>
    {

    }

    /// <summary>
    /// 处理特殊时期普通弹幕的 <see cref="IBilibiliJsonMessageParser{TMessage}"/>
    /// </summary>
    public class DanmakuFallbackParser<TMessage, TImpl> : DanmakuParserBase<TMessage, TImpl> where TMessage : IDanmakuMessage
                                                                                             where TImpl : DanmakuMessage, TMessage, new()
    {
        /// <inheritdoc/>
        public override bool CanParse(JsonElement root)
        {
            return root.TryGetProperty("cmd", out JsonElement commandToken) && (commandToken.GetString()?.StartsWith(Command) ?? false);
        }
    }
}
