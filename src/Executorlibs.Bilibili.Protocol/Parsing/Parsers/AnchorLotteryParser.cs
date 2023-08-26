using System;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;
using Executorlibs.Shared;

namespace Executorlibs.Bilibili.Protocol.Parsing.Parsers
{
    public class AnchorLotteryParser : AnchorLotteryParser<IAnchorLotteryMessage, AnchorLotteryMessage>
    {
        /// <summary>
        /// 初始化 <see cref="AnchorLotteryParser"/> 类的新实例
        /// </summary>
        public AnchorLotteryParser() { }
    }

    public class AnchorLotteryParser<TMessage, TImpl> : BilibiliMappableJsonMessageParser<TMessage, TImpl> where TMessage : IAnchorLotteryMessage
                                                                                                           where TImpl : AnchorLotteryMessage, TMessage, new()
    {
        private const string Command = "ANCHOR_LOT_START";

        /// <inheritdoc/>
        public override string Key => Command;

        /// <summary>
        /// 初始化 <see cref="AnchorLotteryParser{TMessage, TImpl}"/> 类的新实例
        /// </summary>
        public AnchorLotteryParser() { }

        /// <summary>
        /// 将给定的 <paramref name="rawdata"/> 处理为 <typeparamref name="TMessage"/> 实例
        /// </summary>
        /// <param name="rawdata">消息数据</param>
        protected override TImpl CreateMessage(JsonElement rawdata)
        {
            var message = base.CreateMessage(rawdata);
            var data = rawdata.GetProperty("data");
            message.Id = data.GetProperty("id").GetUInt32();
            message.Danmaku = data.GetProperty("danmu").GetString()!;
            message.AwardName = data.GetProperty("award_name").GetString()!;
            message.AwardNum = data.GetProperty("award_num").GetUInt32();
            message.GiftName = data.GetProperty("gift_name").GetString();
            message.GiftNum = data.GetProperty("gift_num").GetUInt32();
            message.GiftCost = data.GetProperty("gift_price").GetUInt32();
            message.RequireType = data.GetProperty("require_type").GetUInt32();
            message.RequireValue = data.GetProperty("require_value").GetUInt32();
            message.Duration = TimeSpan.FromSeconds(data.GetProperty("max_time").GetInt32());
            message.EndTime = Utils.UnixTime2DateTime(data.GetProperty("current_time").GetInt32()).AddSeconds(data.GetProperty("time").GetInt32());
            if (string.IsNullOrEmpty(message.GiftName))
            {
                message.GiftName = null;
                message.GiftNum = null;
                message.GiftCost = null;
            }
            else
            {
                message.GiftCost *= message.GiftNum;
            }
            message.Time = message.EndTime.Add(-message.Duration);
            return message;
        }
    }
}
