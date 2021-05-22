using System;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;

namespace Executorlibs.Bilibili.Protocol.Parsers
{
    public class RedPocketParser : RedPocketParser<IRedPocketMessage, RedPocketMessage>
    {
        /// <summary>
        /// 初始化 <see cref="RedPocketParser"/> 类的新实例
        /// </summary>
        public RedPocketParser() { }
    }

    public class RedPocketParser<TMessage, TImpl> : BilibiliMappableParser<TMessage> where TMessage : IRedPocketMessage
                                                                                     where TImpl : RedPocketMessage, TMessage, new()
    {
        private const string Command = "RED_POCKET_START";

        /// <inheritdoc/>
        public override string Key => Command;

        /// <summary>
        /// 初始化 <see cref="RedPocketParser{TMessage, TImpl}"/> 类的新实例
        /// </summary>
        public RedPocketParser() { }

        /// <summary>
        /// 将给定的 <paramref name="root"/> 处理为 <typeparamref name="TMessage"/> 实例
        /// </summary>
        /// <param name="root">消息数据</param>
        public override TMessage Parse(in JsonElement root)
        {
            JsonElement data = root.GetProperty("data");
            return new TImpl
            {
                Id = data.GetProperty("id").GetInt32(),
                Count = data.GetProperty("num").GetInt32(),
                Content = data.GetProperty("content").GetString()!,
                Requirement = data.GetProperty("require_message").GetString()!,
                Sender = data.GetProperty("sender_name").GetString()!,
                SenderId = data.GetProperty("sender_uid").GetInt32(),
                Duration = TimeSpan.FromSeconds(data.GetProperty("remain_time").GetInt32()),
                Time = DateTime.Now,
                Rawdata = root
            };
        }
    }
}
