using System;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;

namespace Executorlibs.Bilibili.Protocol.Parsing.Parsers
{
    public class BeatstormParser : BeatstormParser<IBeatstormMessage, BeatstormMessage>
    {
        /// <summary>
        /// 初始化 <see cref="BeatstormParser"/> 类的新实例
        /// </summary>
        public BeatstormParser() { }
    }

    public class BeatstormParser<TMessage, TImpl> : BilibiliMappableJsonMessageParser<TMessage, TImpl> where TMessage : IBeatstormMessage
                                                                                                       where TImpl : BeatstormMessage, TMessage, new()
    {
        private const string Command = "SPECIAL_GIFT";

        /// <inheritdoc/>
        public override string Key => Command;

        /// <summary>
        /// 初始化 <see cref="BeatstormParser{TMessage, TImpl}"/> 类的新实例
        /// </summary>
        public BeatstormParser() { }

        protected override TImpl CreateMessage(JsonElement rawdata)
        {
            var message = base.CreateMessage(rawdata);
            var data39 = rawdata.GetProperty("data").GetProperty("39");
            message.Id = ulong.Parse(data39.GetProperty("id").ToString()!);
            message.Action = data39.GetProperty("action").GetString() == "start";
            if (message.Action)
            {
                message.Content = data39.GetProperty("content").GetString();
                message.Count = data39.GetProperty("num").GetUInt32();
            }
            message.Time = DateTime.Now;
            return message;
        }
    }
}
