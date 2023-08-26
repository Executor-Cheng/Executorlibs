using System;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;
using Executorlibs.Shared;

namespace Executorlibs.Bilibili.Protocol.Parsing.Parsers
{
    /// <summary>
    /// 处理用户进入直播间消息的 <see cref="IBilibiliJsonMessageParser{TMessage}"/>
    /// </summary>
    public sealed class LiveStartParser : LiveStartParser<ILiveStartMessage, LiveStartMessage>
    {

    }

    /// <summary>
    /// 处理主播开播消息的 <see cref="IBilibiliJsonMessageParser{TMessage}"/>
    /// </summary>
    public class LiveStartParser<TMessage, TImpl> : BilibiliMappableJsonMessageParser<TMessage, TImpl> where TMessage : ILiveStartMessage
                                                                                                       where TImpl : LiveStartMessage, TMessage, new()
    {
        private const string Command = "LIVE";

        /// <inheritdoc/>
        public override string Key => Command;

        /// <summary>
        /// 初始化 <see cref="LiveStartParser{TMessage, TImpl}"/> 类的新实例
        /// </summary>
        public LiveStartParser() { }

        protected override TImpl CreateMessage(JsonElement rawdata)
        {
            var message = base.CreateMessage(rawdata);
            message.Time = Utils.UnixTime2DateTime(rawdata.GetProperty("live_time").GetInt32());
            message.Rawdata = rawdata;
            return message;
        }
    }
}
