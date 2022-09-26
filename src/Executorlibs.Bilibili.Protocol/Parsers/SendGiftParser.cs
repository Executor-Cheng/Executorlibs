using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;
using Executorlibs.Bilibili.Protocol.Models.Enums;
using Executorlibs.Shared;

namespace Executorlibs.Bilibili.Protocol.Parsers
{
    /// <summary>
    /// 处理赠送礼物弹幕的 <see cref="IBilibiliMessageParser{TMessage}"/>
    /// </summary>
    public sealed class SendGiftParser : SendGiftParser<ISendGiftMessage, SendGiftMessage>
    {

    }

    /// <summary>
    /// 处理赠送礼物弹幕的 <see cref="IBilibiliMessageParser{TMessage}"/>
    /// </summary>
    public class SendGiftParser<TMessage, TImpl> : BilibiliMappableMessageParser<TMessage> where TMessage : ISendGiftMessage
                                                                                    where TImpl : SendGiftMessage, TMessage, new()
    {
        private const string Command = "SEND_GIFT";

        /// <inheritdoc/>
        public override string Key => Command;

        /// <summary>
        /// 初始化 <see cref="SendGiftParser{TMessage, TImpl}"/> 类的新实例
        /// </summary>
        public SendGiftParser() { }

        /// <summary>
        /// 将给定的 <paramref name="root"/> 处理为 <typeparamref name="TMessage"/> 实例
        /// </summary>
        /// <remarks>
        /// <see cref="Executorlibs.Shared.Protocol.Models.Danmaku.IMedal.Master"/> 恒为 <see langword="null"/>; <see cref="Executorlibs.Shared.Protocol.Models.Danmaku.IMedal.RoomId"/> 恒为 0;
        /// </remarks>
        /// <param name="root">消息数据</param>
        public override TMessage Parse(in JsonElement root)
        {
            JsonElement data = root.GetProperty("data"),
                        medal = data.GetProperty("medal_info");
            long targetId = medal.GetProperty("target_id").GetInt64();
            return new TImpl
            {
                Id = long.Parse(data.GetProperty("tid").GetString()!),
                UserName = data.GetProperty("uname").GetString()!,
                UserId = data.GetProperty("uid").GetInt64(),
                GiftId = data.GetProperty("giftId").GetInt32(),
                GiftName = data.GetProperty("giftName").GetString()!,
                GiftCount = data.GetProperty("num").GetInt32(),
                GuardType = (GuardType)data.GetProperty("guard_level").GetInt32(),
                Time = Utils.UnixTime2DateTime(data.GetProperty("timestamp").GetInt32()),
                GiftSeedPrice = data.GetProperty("price").GetInt32(),
                IsGoldGift = data.GetProperty("coin_type").GetString() == "gold",
                RoomCost = data.GetProperty("rcost").GetInt64(),
                Medal = targetId != 0 ? new Medal
                {
                    Badge = medal.GetProperty("icon_id").GetInt32(),
                    Color = medal.GetProperty("medal_color").GetInt32(),
                    Level = medal.GetProperty("medal_level").GetInt32(),
                    Master = null!, // 破站不给啊, 老懒鬼了
                    MasterId = targetId,
                    Name = medal.GetProperty("medal_name").GetString()!,
                    RoomId = 0 // 同上
                } : null,
                Rawdata = root
            };
        }
    }
}
