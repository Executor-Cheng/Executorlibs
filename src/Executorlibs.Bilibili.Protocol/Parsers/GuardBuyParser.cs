using System;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;
using Executorlibs.Bilibili.Protocol.Models.Enums;

namespace Executorlibs.Bilibili.Protocol.Parsers
{
    /// <summary>
    /// 处理购买船员消息的 <see cref="IBilibiliMessageParser{TMessage}"/>
    /// </summary>
    public class GuardBuyParser : GuardBuyParser<IGuardBuyMessage, GuardBuyMessage>
    {
        /// <summary>
        /// 初始化 <see cref="GuardBuyParser"/> 类的新实例
        /// </summary>
        public GuardBuyParser() { }
    }

    public class GuardBuyParser<TMessage, TImpl> : BilibiliMappableParser<TMessage> where TMessage : IGuardBuyMessage
                                                                                           where TImpl : GuardBuyMessage, TMessage, new()
    {
        private const string Command = "GUARD_BUY";

        /// <inheritdoc/>
        public override string Key => Command;

        /// <summary>
        /// 初始化 <see cref="GuardBuyParser{TMessage, TImpl}"/> 类的新实例
        /// </summary>
        public GuardBuyParser() { }

        /// <summary>
        /// 将给定的 <paramref name="root"/> 处理为 <typeparamref name="TMessage"/> 实例
        /// </summary>
        /// <param name="root">消息数据</param>
        public override TMessage Parse(in JsonElement root)
        {
            JsonElement data = root.GetProperty("data");
            TImpl message = new TImpl
            {
                UserName = data.GetProperty("username").GetString()!,
                UserId = data.GetProperty("uid").GetInt32(),
                GuardType = (GuardType)data.GetProperty("guard_level").GetInt32(),
                GiftCount = data.GetProperty("num").GetInt32(),
                IsGoldGift = true,
                Time = DateTime.Now,
                Rawdata = root
            };
            message.GiftName = message.GuardType switch
            {
                GuardType.Captain => "舰长",
                GuardType.Praefect => "提督",
                GuardType.Governor => "总督",
                GuardType.WeekCaptain => "周舰长",
                _ => "未知"
            };
            message.GiftId = message.GuardType switch
            {
                GuardType.Captain => 10003,
                GuardType.Praefect => 10002,
                GuardType.Governor => 10001,
                GuardType.WeekCaptain => 10004,
                _ => 0
            };
            return message;
        }
    }
}
