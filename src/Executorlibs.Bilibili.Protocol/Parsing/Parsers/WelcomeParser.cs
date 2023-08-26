using System;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;
using Executorlibs.Bilibili.Protocol.Models.Enums;

namespace Executorlibs.Bilibili.Protocol.Parsing.Parsers
{
    /// <summary>
    /// 处理欢迎老爷进入房间消息的 <see cref="IBilibiliJsonMessageParser{TMessage}"/>
    /// </summary>
    public class WelcomeParser : WelcomeParser<IWelcomeMessage, WelcomeMessage>
    {
        /// <summary>
        /// 初始化 <see cref="WelcomeParser"/> 类的新实例
        /// </summary>
        public WelcomeParser() { }
    }

    /// <summary>
    /// 处理欢迎老爷进入房间消息的 <see cref="IBilibiliJsonMessageParser{TMessage}"/>
    /// </summary>
    public class WelcomeParser<TMessage, TImpl> : BilibiliMappableJsonMessageParser<TMessage, TImpl> where TMessage : IWelcomeMessage
                                                                                                     where TImpl : WelcomeMessage, TMessage, new()
    {
        private const string Command = "WELCOME";

        /// <inheritdoc/>
        public override string Key => Command;

        /// <summary>
        /// 初始化 <see cref="WelcomeParser{TMessage, TImpl}"/> 类的新实例
        /// </summary>
        public WelcomeParser() { }

        protected override TImpl CreateMessage(JsonElement rawdata)
        {
            var message = base.CreateMessage(rawdata);
            var data = rawdata.GetProperty("data");
            message.UserName = data.GetProperty("uname").GetString()!;
            message.UserId = data.GetProperty("uid").GetUInt64();
            message.LordType = data.TryGetProperty("svip", out var svip) && svip.GetInt32() == 1 ? LordType.Yearly : LordType.Monthly;
            message.IsAdmin = data.TryGetProperty("is_admin", out var admin) ? admin.GetBoolean() :
                              data.TryGetProperty("isadmin", out admin) ? admin.GetUInt32() == 1 :
                              data.TryGetProperty("isAdmin", out admin) && admin.GetUInt32() == 1; // ......
            message.Time = DateTime.Now;
            return message;
        }
    }
}
