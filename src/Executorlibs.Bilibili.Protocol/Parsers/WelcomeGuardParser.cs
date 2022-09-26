using System;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;
using Executorlibs.Bilibili.Protocol.Models.Enums;

namespace Executorlibs.Bilibili.Protocol.Parsers
{
    /// <summary>
    /// 处理欢迎船员进入房间消息的 <see cref="IBilibiliMessageParser{TMessage}"/>
    /// </summary>
    public class WelcomeGuardParser : BilibiliMappableMessageParser<IWelcomeGuardMessage>
    {
        private const string Command = "WELCOME_GUARD";

        /// <inheritdoc/>
        public override string Key => Command;

        /// <summary>
        /// 初始化 <see cref="WelcomeGuardParser"/> 类的新实例
        /// </summary>
        public WelcomeGuardParser() { }

        /// <summary>
        /// 将给定的 <paramref name="root"/> 处理为 <see cref="IWelcomeGuardMessage"/> 实例
        /// </summary>
        /// <param name="root">消息数据</param>
        public override IWelcomeGuardMessage Parse(in JsonElement root)
        {
            JsonElement data = root.GetProperty("data");
            return new WelcomeGuardMessage
            {
                UserName = data.GetProperty("username").GetString()!,
                UserId = data.GetProperty("uid").GetInt64(),
                GuardType = (GuardType)data.GetProperty("guard_level").GetInt32(),
                Time = DateTime.Now,
                Rawdata = root
            };
        }
    }
}
