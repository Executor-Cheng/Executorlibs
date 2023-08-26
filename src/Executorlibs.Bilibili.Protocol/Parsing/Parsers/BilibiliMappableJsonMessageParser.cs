using System.Runtime.CompilerServices;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.General;

namespace Executorlibs.Bilibili.Protocol.Parsing.Parsers
{
    public interface IBilibiliMappableJsonMessageParser<out TMessage> : IBilibiliJsonMessageParser<TMessage> where TMessage : IBilibiliJsonMessage
    {
        /// <summary>
        /// 表示弹幕数据中的 cmd 值
        /// </summary>
        string Key { get; }
    }

    /// <summary>
    /// 以 cmd 作为键的 <see cref="IBilibiliJsonMessageParser{TMessage}"/>
    /// </summary>
    /// <remarks>
    /// 仅供 Bilibili 直播平台的消息使用
    /// </remarks>
    /// <typeparam name="TMessage"></typeparam>
    /// <typeparam name="TMessageImpl"></typeparam>
    public abstract class BilibiliMappableJsonMessageParser<TMessage, TMessageImpl> : BilibiliJsonMessageParser<TMessage, TMessageImpl>,
                                                                                      IBilibiliMappableJsonMessageParser<TMessage> where TMessage : IBilibiliJsonMessage
                                                                                      where TMessageImpl : BilibiliJsonMessage, TMessage, new()
    {
        public abstract string Key { get; }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool CanParse(JsonElement root)
        {
            return true;
        }
    }
}
