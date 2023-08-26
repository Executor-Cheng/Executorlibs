using System.Runtime.CompilerServices;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;

namespace Executorlibs.Bilibili.Protocol.Parsing.Parsers
{
    /// <summary>
    /// 处理用户进入直播间消息的 <see cref="IBilibiliJsonMessageParser{TMessage}"/>
    /// </summary>
    public sealed class EnterParser : EnterParser<IEnterMessage, EnterMessage>
    {

    }

    /// <summary>
    /// 处理用户进入直播间消息的 <see cref="IBilibiliJsonMessageParser{TMessage}"/>
    /// </summary>
    public class EnterParser<TMessage, TImpl> : InteractBaseParser<TMessage, TImpl> where TMessage : IEnterMessage
                                                                                    where TImpl : EnterMessage, TMessage, new()
    {
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool CanParse(JsonElement root)
        {
            return base.CanParse(root) &&
                   root.TryGetProperty("data", out JsonElement data) &&
                   data.TryGetProperty("msg_type", out JsonElement msgType) &&
                   msgType.TryGetInt32(out int value) &&
                   value == 1;
        }
    }
}
