using System.Linq;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;
using Executorlibs.Bilibili.Protocol.Models.Enums;
using Executorlibs.Shared;

namespace Executorlibs.Bilibili.Protocol.Parsing.Parsers
{
    /// <summary>
    /// 处理用户互动消息的 <see cref="IBilibiliJsonMessageParser{TMessage}"/>
    /// </summary>
    public abstract class InteractBaseParser<TMessage, TImpl> : BilibiliMappableJsonMessageParser<TMessage, TImpl> where TMessage : IInteractMessage
                                                                                                                   where TImpl : InteractMessage, TMessage, new()
    {
        /// <summary>
        /// 本消息的cmd值
        /// </summary>
        protected const string Command = "INTERACT_WORD";

        /// <inheritdoc/>
        public override string Key => Command;

        protected override TImpl CreateMessage(JsonElement rawdata)
        {
            var message = base.CreateMessage(rawdata);
            var data = rawdata.GetProperty("data");
            message.Type = (InteractType)data.GetProperty("msg_type").GetInt32();
            message.UserType = (InteractUserType)data.GetProperty("identities").EnumerateArray().Select(p => p.GetInt32()).Aggregate(0, (t, c) => t | 1 << (c - 1));
            message.UserName = data.GetProperty("uname").GetString()!;
            message.UserId = data.GetProperty("uid").GetUInt64();
            message.Time = Utils.UnixTime2DateTime(data.GetProperty("timestamp").GetInt32());
            return message;
        }
    }
}
