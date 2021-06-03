using System;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;
using Executorlibs.Bilibili.Protocol.Models.Enums;

namespace Executorlibs.Bilibili.Protocol.Parsers
{
    /// <summary>
    /// 处理欢迎老爷进入房间消息的 <see cref="IBilibiliMessageParser{TMessage}"/>
    /// </summary>
    public class WelcomeParser : WelcomeParser<IWelcomeMessage, WelcomeMessage>
    {
        /// <summary>
        /// 初始化 <see cref="WelcomeParser"/> 类的新实例
        /// </summary>
        public WelcomeParser() { }
    }

    /// <summary>
    /// 处理欢迎老爷进入房间消息的 <see cref="IBilibiliMessageParser{TMessage}"/>
    /// </summary>
    public class WelcomeParser<TMessage, TImpl> : BilibiliMappableParser<TMessage> where TMessage : IWelcomeMessage
                                                                                   where TImpl : WelcomeMessage, TMessage, new()
    {
        private const string Command = "WELCOME";

        /// <inheritdoc/>
        public override string Key => Command;

        /// <summary>
        /// 初始化 <see cref="WelcomeParser{TMessage, TImpl}"/> 类的新实例
        /// </summary>
        public WelcomeParser() { }

        /// <summary>
        /// 将给定的 <paramref name="root"/> 处理为 <see cref="IWelcomeMessage"/> 实例
        /// </summary>
        /// <param name="root">消息数据</param>
        public override TMessage Parse(in JsonElement root)
        {
            JsonElement data = root.GetProperty("data");
            return new TImpl
            {
                UserName = data.GetProperty("uname").GetString()!,
                UserId = data.GetProperty("uid").GetInt32(),
                LordType = data.TryGetProperty("svip", out JsonElement svip) && svip.GetInt32() == 1 ? LordType.Yearly : LordType.Monthly,
                IsAdmin = data.TryGetProperty("is_admin", out JsonElement admin) ? admin.GetBoolean() :
                          data.TryGetProperty("isadmin", out admin) ? admin.GetInt32() == 1 :
                          data.TryGetProperty("isAdmin", out admin) && admin.GetInt32() == 1,
                Time = DateTime.Now,
                Rawdata = root
            };
        }
    }
}
