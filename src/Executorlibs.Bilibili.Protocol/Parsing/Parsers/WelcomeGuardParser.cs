using System;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;
using Executorlibs.Bilibili.Protocol.Models.Enums;

namespace Executorlibs.Bilibili.Protocol.Parsing.Parsers
{
    /// <summary>
    /// 处理欢迎船员进入房间消息的 <see cref="IBilibiliJsonMessageParser{TMessage}"/>
    /// </summary>
    public class WelcomeGuardParser : BilibiliMappableJsonMessageParser<IWelcomeGuardMessage, WelcomeGuardMessage>
    {
        private const string Command = "WELCOME_GUARD";

        /// <inheritdoc/>
        public override string Key => Command;

        /// <summary>
        /// 初始化 <see cref="WelcomeGuardParser"/> 类的新实例
        /// </summary>
        public WelcomeGuardParser() { }

        protected override WelcomeGuardMessage CreateMessage(JsonElement rawdata)
        {
            var message = base.CreateMessage(rawdata);
            var data = rawdata.GetProperty("data");
            message.UserName = data.GetProperty("username").GetString()!;
            message.UserId = data.GetProperty("uid").GetUInt64();
            message.GuardType = (GuardType)data.GetProperty("guard_level").GetInt32();
            message.Time = DateTime.Now;
            return message;
        }
    }
}
