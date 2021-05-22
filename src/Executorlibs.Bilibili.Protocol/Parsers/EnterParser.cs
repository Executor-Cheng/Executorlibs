using System.Runtime.CompilerServices;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;

namespace Executorlibs.Bilibili.Protocol.Parsers
{
    /// <summary>
    /// 处理用户进入直播间消息的 <see cref="IBilibiliMessageParser{TMessage}"/>
    /// </summary>
    public sealed class EnterParser : DefaultEnterParser<IEnterMessage, EnterMessage>
    {
        
    }

    /// <summary>
    /// 处理用户进入直播间消息的 <see cref="IBilibiliMessageParser{TMessage}"/>
    /// </summary>
    public class DefaultEnterParser<TMessage, TImpl> : InteractBaseParser<TMessage, TImpl> where TMessage : IEnterMessage
                                                                                           where TImpl : EnterMessage, TMessage, new()
    {
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool CanParse(in JsonElement root)
        {
            return base.CanParse(in root) &&
                   root.TryGetProperty("data", out JsonElement data) &&
                   data.TryGetProperty("msg_type", out JsonElement msgType) &&
                   msgType.TryGetInt32(out int value) &&
                   value == 1;
        }
    }
}
