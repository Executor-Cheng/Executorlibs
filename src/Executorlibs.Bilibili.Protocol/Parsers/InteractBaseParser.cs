using System.Linq;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;
using Executorlibs.Bilibili.Protocol.Models.Enums;
using Executorlibs.Shared;

namespace Executorlibs.Bilibili.Protocol.Parsers
{
    /// <summary>
    /// 处理用户互动消息的 <see cref="IBilibiliMessageParser{TMessage}"/>
    /// </summary>
    public abstract class InteractBaseParser<TMessage, TImpl> : BilibiliMappableMessageParser<TMessage> where TMessage : IInteractMessage
                                                                                                 where TImpl : InteractMessage, TMessage, new()
    {
        /// <summary>
        /// 本消息的cmd值
        /// </summary>
        protected const string Command = "INTERACT_WORD";

        /// <inheritdoc/>
        public override string Key => Command;

        /// <summary>
        /// 将给定的 <paramref name="root"/> 处理为 <typeparamref name="TMessage"/> 实例
        /// </summary>
        /// <param name="root">消息数据</param>
        public override TMessage Parse(in JsonElement root)
        {
            JsonElement data = root.GetProperty("data");
            return new TImpl
            {
                Type = (InteractType)data.GetProperty("msg_type").GetInt32(),
                UserType = (InteractUserType)data.GetProperty("identities").EnumerateArray().Select(p => p.GetInt32()).Aggregate(0, (t, c) => t | 1 << (c - 1)),
                UserName = data.GetProperty("uname").GetString()!,
                UserId = data.GetProperty("uid").GetInt32(),
                Time = Utils.UnixTime2DateTime(data.GetProperty("timestamp").GetInt32()),
                Rawdata = root
            };
        }
    }
}
