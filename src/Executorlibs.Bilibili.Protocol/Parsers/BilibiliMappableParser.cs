using System.Runtime.CompilerServices;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.General;

namespace Executorlibs.Bilibili.Protocol.Parsers
{
    /// <summary>
    /// 以 cmd 作为键的 <see cref="IBilibiliMessageParser{TMessage}"/>
    /// </summary>
    /// <remarks>
    /// 仅供 Bilibili 直播平台的消息使用
    /// </remarks>
    /// <typeparam name="TMessage"></typeparam>
    public abstract class BilibiliMappableParser<TMessage> : IMappableBilibiliMessageParser<TMessage> where TMessage : IBilibiliMessage
    {
        /// <summary>
        /// 表示弹幕数据中的 cmd 值
        /// </summary>
        public abstract string Key { get; }

        /// <summary>
        /// 测试给定的 <typeparamref name="TMessage"/> 能否被处理。
        /// </summary>
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool CanParse(in JsonElement root)
        {
            return root.TryGetProperty("cmd", out JsonElement commandToken) && commandToken.GetString() == Key;
        }

        /// <inheritdoc/>
        public abstract TMessage Parse(in JsonElement root);
    }
}
