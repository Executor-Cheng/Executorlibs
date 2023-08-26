using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;
using Executorlibs.Bilibili.Protocol.Models.Enums;
using Executorlibs.Shared;

namespace Executorlibs.Bilibili.Protocol.Parsing.Parsers
{
    /// <summary>
    /// 处理赠送礼物弹幕的 <see cref="IBilibiliJsonMessageParser{TMessage}"/>
    /// </summary>
    public sealed class SendGiftParser : SendGiftParser<ISendGiftMessage, SendGiftMessage>
    {

    }

    /// <summary>
    /// 处理赠送礼物弹幕的 <see cref="IBilibiliJsonMessageParser{TMessage}"/>
    /// </summary>
    public class SendGiftParser<TMessage, TImpl> : BilibiliMappableJsonMessageParser<TMessage, TImpl> where TMessage : ISendGiftMessage
                                                                                                      where TImpl : SendGiftMessage, TMessage, new()
    {
        private const string Command = "SEND_GIFT";

        /// <inheritdoc/>
        public override string Key => Command;

        /// <summary>
        /// 初始化 <see cref="SendGiftParser{TMessage, TImpl}"/> 类的新实例
        /// </summary>
        public SendGiftParser() { }

        protected override TImpl CreateMessage(JsonElement rawdata)
        {
            var message = base.CreateMessage(rawdata);
            JsonElement data = rawdata.GetProperty("data"),
                        medal = data.GetProperty("medal_info");
            ulong targetId = medal.GetProperty("target_id").GetUInt64();
            message.Id = ulong.Parse(data.GetProperty("tid").GetString()!);
            message.UserName = data.GetProperty("uname").GetString()!;
            message.UserId = data.GetProperty("uid").GetUInt64();
            message.GiftId = data.GetProperty("giftId").GetUInt32();
            message.GiftName = data.GetProperty("giftName").GetString()!;
            message.GiftCount = data.GetProperty("num").GetUInt32();
            message.GuardType = (GuardType)data.GetProperty("guard_level").GetInt32();
            message.Time = Utils.UnixTime2DateTime(data.GetProperty("timestamp").GetInt32());
            message.GiftSeedPrice = data.GetProperty("price").GetUInt32();
            message.IsGoldGift = data.GetProperty("coin_type").GetString() == "gold";
            message.RoomCost = data.GetProperty("rcost").GetUInt64();
            message.Medal = targetId != 0 ? new Medal
            {
                Badge = medal.GetProperty("icon_id").GetUInt32(),
                Color = medal.GetProperty("medal_color").GetUInt32(),
                Level = medal.GetProperty("medal_level").GetUInt32(),
                Master = null!, // 破站不给啊, 老懒鬼了
                MasterId = targetId,
                Name = medal.GetProperty("medal_name").GetString()!,
                RoomId = 0 // 同上
            } : null;
            return message;
        }
    }
}
