using System;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;
using Executorlibs.Shared;

namespace Executorlibs.Bilibili.Protocol.Parsers
{
    /// <summary>
    /// 处理用户进入直播间消息的 <see cref="IBilibiliMessageParser{TMessage}"/>
    /// </summary>
    public sealed class LiveStartParser : LiveStartParser<ILiveStartMessage, LiveStartMessage>
    {

    }

    /// <summary>
    /// 处理主播开播消息的 <see cref="IBilibiliMessageParser{TMessage}"/>
    /// </summary>
    public class LiveStartParser<TMessage, TImpl> : BilibiliMappableMessageParser<TMessage> where TMessage : ILiveStartMessage
                                                                                            where TImpl : LiveStartMessage, TMessage, new()
    {
        private const string Command = "LIVE";

        /// <inheritdoc/>
        public override string Key => Command;

        /// <summary>
        /// 初始化 <see cref="LiveStartParser{TMessage, TImpl}"/> 类的新实例
        /// </summary>
        public LiveStartParser() { }

        public override TMessage Parse(in JsonElement root)
        {
            TImpl result = new TImpl();
#if NETSTANDARD2_0
            string subSessionKey = root.GetProperty("sub_session_key").GetString()!;
            result.Time = Utils.UnixTime2DateTime(int.Parse(subSessionKey.Substring(subSessionKey.IndexOf(':'))));
#else
            ReadOnlySpan<char> subSessionKey = root.GetProperty("sub_session_key").GetString()!; // 181815281423909749sub_time:1632584495
            result.Time = Utils.UnixTime2DateTime(int.Parse(subSessionKey[(subSessionKey.IndexOf(':') + 1)..]));
#endif
            result.Rawdata = root;
            return result;
        }
    }
}
