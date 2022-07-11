using System;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;

namespace Executorlibs.Bilibili.Protocol.Parsers
{
    public class OnlineCountParser<TMessage, TImpl> : BilibiliMappableMessageParser<TMessage> where TMessage : IOnlineCountMessage
                                                                                              where TImpl : OnlineCountMessage, TMessage, new()
    {
        private const string Command = "ONLINE_RANK_COUNT";

        /// <inheritdoc/>
        public override string Key => Command;

        /// <summary>
        /// 初始化 <see cref="OnlineCountParser{TMessage, TImpl}"/> 类的新实例
        /// </summary>
        public OnlineCountParser() { }

        /// <summary>
        /// 将给定的 <paramref name="root"/> 处理为 <typeparamref name="TMessage"/> 实例
        /// </summary>
        /// <param name="root">消息数据</param>
        public override TMessage Parse(in JsonElement root)
        {
            JsonElement data = root.GetProperty("data");
            TImpl message = new TImpl();
            message.Count = data.GetProperty("count").GetInt32();
            message.Rawdata = root;
            message.Time = DateTime.Now;
            return message;
        }
    }
}
