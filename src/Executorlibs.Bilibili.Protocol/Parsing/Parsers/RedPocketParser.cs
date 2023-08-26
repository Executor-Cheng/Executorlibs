using System;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;

namespace Executorlibs.Bilibili.Protocol.Parsing.Parsers
{
    public class RedPocketParser : RedPocketParser<IRedPocketMessage, RedPocketMessage>
    {
        /// <summary>
        /// 初始化 <see cref="RedPocketParser"/> 类的新实例
        /// </summary>
        public RedPocketParser() { }
    }

    public class RedPocketParser<TMessage, TImpl> : BilibiliMappableJsonMessageParser<TMessage, TImpl> where TMessage : IRedPocketMessage
                                                                                                       where TImpl : RedPocketMessage, TMessage, new()
    {
        private const string Command = "RED_POCKET_START";

        /// <inheritdoc/>
        public override string Key => Command;

        /// <summary>
        /// 初始化 <see cref="RedPocketParser{TMessage, TImpl}"/> 类的新实例
        /// </summary>
        public RedPocketParser() { }

        protected override TImpl CreateMessage(JsonElement rawdata)
        {
            var message = base.CreateMessage(rawdata);
            var data = rawdata.GetProperty("data");
            message.Id = data.GetProperty("id").GetUInt32();
            message.Count = data.GetProperty("num").GetUInt32();
            message.Content = data.GetProperty("content").GetString()!;
            message.Requirement = data.GetProperty("require_message").GetString()!;
            message.Sender = data.GetProperty("sender_name").GetString()!;
            message.SenderId = data.GetProperty("sender_uid").GetUInt64();
            message.Duration = TimeSpan.FromSeconds(data.GetProperty("remain_time").GetInt32());
            message.Time = DateTime.Now;
            return message;
        }
    }
}
