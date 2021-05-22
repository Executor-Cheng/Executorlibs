using System.Runtime.CompilerServices;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;

namespace Executorlibs.Bilibili.Protocol.Parsers
{
    /// <summary>
    /// 处理用户关注直播间消息的 <see cref="IBilibiliMessageParser{TMessage}"/>
    /// </summary>
    public sealed class FollowParser : InteractBaseParser<IFollowMessage, FollowMessage>
    {
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool CanParse(in JsonElement root)
        {
            return base.CanParse(in root) &&
                   root.TryGetProperty("data", out JsonElement data) &&
                   data.TryGetProperty("msg_type", out JsonElement msgType) &&
                   msgType.TryGetInt32(out int value) &&
                   value == 2;
        }
    }
}
