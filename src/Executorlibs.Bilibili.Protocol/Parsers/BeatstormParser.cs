using System;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;

namespace Executorlibs.Bilibili.Protocol.Parsers
{
    public class BeatstormParser : BeatstormParser<IBeatstormMessage, BeatstormMessage>
    {
        /// <summary>
        /// 初始化 <see cref="BeatstormParser"/> 类的新实例
        /// </summary>
        public BeatstormParser() { }
    }

    public class BeatstormParser<TMessage, TImpl> : BilibiliMappableMessageParser<TMessage> where TMessage : IBeatstormMessage
                                                                                            where TImpl : BeatstormMessage, TMessage, new()
    {
        private const string Command = "SPECIAL_GIFT";

        /// <inheritdoc/>
        public override string Key => Command;

        /// <summary>
        /// 初始化 <see cref="BeatstormParser{TMessage, TImpl}"/> 类的新实例
        /// </summary>
        public BeatstormParser() { }

        /// <inheritdoc/>
        public override bool CanParse(in JsonElement root)
        {
            return root.TryGetProperty("data", out JsonElement data) &&
                   data.TryGetProperty("39", out _);
        }

        /// <summary>
        /// 将给定的 <paramref name="root"/> 处理为 <typeparamref name="TMessage"/> 实例
        /// </summary>
        /// <param name="root">消息数据</param>
        public override TMessage Parse(in JsonElement root)
        {
            JsonElement data = root.GetProperty("data");
            JsonElement data_39 = data.GetProperty("39");
            TImpl message = new TImpl();
            message.Id = long.Parse(data_39.GetProperty("id").ToString()!);
            message.Action = data_39.GetProperty("action").GetString() == "start";
            if (message.Action)
            {
                message.Content = data_39.GetProperty("content").GetString();
                message.Count = data_39.GetProperty("num").GetInt32();
            }
            message.Rawdata = root;
            message.Time = DateTime.Now;
            return message;
        }
    }
}
