using System.Runtime.CompilerServices;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;

namespace Executorlibs.Bilibili.Protocol.Parsers
{
    /// <summary>
    /// 处理用户互动消息的 <see cref="IMappableBilibiliMessageParser{TMessage}"/>
    /// </summary>
    public sealed class InteractParser : InteractBaseParser<IInteractMessage, InteractMessage>, IMappableBilibiliMessageParser<IInteractMessage>
    {
        /// <remarks>
        /// 本方法始终返回 <see langword="true"/>
        /// </remarks>
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool CanParse(in JsonElement root)
        {
            return true;
        }
    }
}
