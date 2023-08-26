using System;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;

namespace Executorlibs.Bilibili.Protocol.Parsing.Parsers
{
    public class OnlineCountParser : OnlineCountParser<IOnlineCountMessage, OnlineCountMessage>
    {
        /// <summary>
        /// 初始化 <see cref="OnlineCountParser"/> 类的新实例
        /// </summary>
        public OnlineCountParser() { }
    }

    public class OnlineCountParser<TMessage, TImpl> : BilibiliMappableJsonMessageParser<TMessage, TImpl> where TMessage : IOnlineCountMessage
                                                                                                         where TImpl : OnlineCountMessage, TMessage, new()
    {
        private const string Command = "ONLINE_RANK_COUNT";

        /// <inheritdoc/>
        public override string Key => Command;

        /// <summary>
        /// 初始化 <see cref="OnlineCountParser{TMessage, TImpl}"/> 类的新实例
        /// </summary>
        public OnlineCountParser() { }

        protected override TImpl CreateMessage(JsonElement rawdata)
        {
            var message = base.CreateMessage(rawdata);
            var data = rawdata.GetProperty("data");
            message.Count = data.GetProperty("count").GetUInt32();
            message.Time = DateTime.Now;
            return message;
        }
    }
}
