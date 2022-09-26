using System;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;

namespace Executorlibs.Bilibili.Protocol.Parsers
{
    /// <summary>
    /// 处理用户被禁言消息的 <see cref="IBilibiliMessageParser{TMessage}"/>
    /// </summary>
    public class UserMutedParser : UserMutedParser<IUserMutedMessage, UserMutedMessage>
    {
        /// <summary>
        /// 初始化 <see cref="UserMutedParser"/> 类的新实例
        /// </summary>
        public UserMutedParser() { }
    }

    /// <summary>
    /// 处理用户被禁言消息的 <see cref="IBilibiliMessageParser{TMessage}"/>
    /// </summary>
    public class UserMutedParser<TMessage, TImpl> : BilibiliMappableMessageParser<TMessage> where TMessage : IUserMutedMessage
                                                                                            where TImpl : UserMutedMessage, TMessage, new()
    {
        private const string Command = "ROOM_BLOCK_MSG";

        /// <inheritdoc/>
        public override string Key => Command;

        /// <summary>
        /// 初始化 <see cref="UserMutedParser{TMessage, TImpl}"/> 类的新实例
        /// </summary>
        public UserMutedParser() { }

        /// <summary>
        /// 将给定的 <paramref name="root"/> 处理为 <typeparamref name="TMessage"/> 实例
        /// </summary>
        /// <param name="root">消息数据</param>
        public override TMessage Parse(in JsonElement root)
        {
            JsonElement userId = root.GetProperty("uid");
            return new TImpl
            {
                UserName = root.GetProperty("uname").GetString()!,
                UserId = userId.ValueKind == JsonValueKind.Number ? userId.GetInt64() : long.Parse(userId.GetString()!), // 破站的程序员真的该死
                MasterOperation = root.GetProperty("data").GetProperty("operator").GetInt32() == 2,
                Time = DateTime.Now,
                Rawdata = root
            };
        }
    }
}
