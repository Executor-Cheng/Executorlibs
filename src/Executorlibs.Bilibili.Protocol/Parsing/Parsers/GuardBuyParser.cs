using System;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;
using Executorlibs.Bilibili.Protocol.Models.Enums;

namespace Executorlibs.Bilibili.Protocol.Parsing.Parsers
{
    /// <summary>
    /// 处理购买船员消息的 <see cref="IBilibiliJsonMessageParser{TMessage}"/>
    /// </summary>
    public class GuardBuyParser : GuardBuyParser<IGuardBuyMessage, GuardBuyMessage>
    {
        
    }

    public class GuardBuyParser<TMessage, TImpl> : BilibiliMappableJsonMessageParser<TMessage, TImpl> where TMessage : IGuardBuyMessage
                                                                                                      where TImpl : GuardBuyMessage, TMessage, new()
    {
        private const string Command = "GUARD_BUY";

        /// <inheritdoc/>
        public override string Key => Command;

        protected override TImpl CreateMessage(JsonElement rawdata)
        {
            var message = base.CreateMessage(rawdata);
            var data = rawdata.GetProperty("data");
            message.UserName = data.GetProperty("username").GetString()!;
            message.UserId = data.GetProperty("uid").GetUInt64();
            message.GuardType = (GuardType)data.GetProperty("guard_level").GetInt32();
            message.GiftCount = data.GetProperty("num").GetUInt32();
            message.IsGoldGift = true;
            message.Time = DateTime.Now;
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
