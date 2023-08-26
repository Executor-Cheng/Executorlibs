using System;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;

namespace Executorlibs.Bilibili.Protocol.Parsing.Parsers
{
    /// <summary>
    /// 处理用户被禁言消息的 <see cref="IBilibiliJsonMessageParser{TMessage}"/>
    /// </summary>
    public class UserMutedParser : UserMutedParser<IUserMutedMessage, UserMutedMessage>
    {
        /// <summary>
        /// 初始化 <see cref="UserMutedParser"/> 类的新实例
        /// </summary>
        public UserMutedParser() { }
    }

    /// <summary>
    /// 处理用户被禁言消息的 <see cref="IBilibiliJsonMessageParser{TMessage}"/>
    /// </summary>
    public class UserMutedParser<TMessage, TImpl> : BilibiliMappableJsonMessageParser<TMessage, TImpl> where TMessage : IUserMutedMessage
                                                                                                       where TImpl : UserMutedMessage, TMessage, new()
    {
        private const string Command = "ROOM_BLOCK_MSG";

        /// <inheritdoc/>
        public override string Key => Command;

        /// <summary>
        /// 初始化 <see cref="UserMutedParser{TMessage, TImpl}"/> 类的新实例
        /// </summary>
        public UserMutedParser() { }

        protected override TImpl CreateMessage(JsonElement rawdata)
        {
            var message = base.CreateMessage(rawdata);
            var userId = rawdata.GetProperty("uid");
            message.UserName = rawdata.GetProperty("uname").GetString()!;
            message.UserId = userId.ValueKind == JsonValueKind.Number ? userId.GetUInt64() : ulong.Parse(userId.GetString()!); // 破站的程序员真的该死
            message.MasterOperation = rawdata.GetProperty("data").GetProperty("operator").GetInt32() == 2;
            message.Time = DateTime.Now;
            return message;
        }
    }
}
