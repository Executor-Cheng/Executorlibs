using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;
using Executorlibs.MessageFramework.Parsers.Attributes;

namespace Executorlibs.Bilibili.Protocol.Parsers
{
    /// <summary>
    /// 处理特殊时期普通弹幕的 <see cref="IBilibiliMessageParser{TMessage}"/>
    /// </summary>
    public sealed class DanmakuFallbackParser : DanmakuFallbackParser<IDanmakuMessage, DanmakuMessage>
    {

    }

    /// <summary>
    /// 处理特殊时期普通弹幕的 <see cref="IBilibiliMessageParser{TMessage}"/>
    /// </summary>
    [SuppressAutoMapping]
    public class DanmakuFallbackParser<TMessage, TImpl> : DanmakuParser<TMessage, TImpl> where TMessage : IDanmakuMessage
                                                                                         where TImpl : DanmakuMessage, TMessage, new()
    {
        /// <inheritdoc/>
        public override bool CanParse(in JsonElement root)
        {
            return root.TryGetProperty("cmd", out JsonElement commandToken) && (commandToken.GetString()?.StartsWith(Key) == true);
        }
    }
}
