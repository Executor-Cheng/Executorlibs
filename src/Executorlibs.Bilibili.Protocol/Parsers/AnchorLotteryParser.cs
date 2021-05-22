using System;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;
using Executorlibs.Shared;

namespace Executorlibs.Bilibili.Protocol.Parsers
{
    public class AnchorLotteryParser : AnchorLotteryParser<IAnchorLotteryMessage, AnchorLotteryMessage>
    {
        /// <summary>
        /// 初始化 <see cref="AnchorLotteryParser"/> 类的新实例
        /// </summary>
        public AnchorLotteryParser() { }
    }

    public class AnchorLotteryParser<TMessage, TImpl> : BilibiliMappableParser<TMessage> where TMessage : IAnchorLotteryMessage
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
        /// 将给定的 <paramref name="root"/> 处理为 <typeparamref name="TMessage"/> 实例
        /// </summary>
        /// <param name="root">消息数据</param>
        public override TMessage Parse(in JsonElement root)
        {
            JsonElement data = root.GetProperty("data");
            TImpl lottery = new TImpl
            {
                Id = data.GetProperty("id").GetInt32(),
                Danmaku = data.GetProperty("danmu").GetString()!,
                AwardName = data.GetProperty("award_name").GetString()!,
                AwardNum = data.GetProperty("award_num").GetInt32(),
                GiftName = data.GetProperty("gift_name").GetString(),
                GiftNum = data.GetProperty("gift_num").GetInt32(),
                GiftCost = data.GetProperty("gift_price").GetInt32(),
                RequireType = data.GetProperty("require_type").GetInt32(),
                RequireValue = data.GetProperty("require_value").GetInt32(),
                Duration = TimeSpan.FromSeconds(data.GetProperty("max_time").GetInt32()),
                EndTime = Utils.UnixTime2DateTime(data.GetProperty("current_time").GetInt32()).AddSeconds(data.GetProperty("time").GetInt32())
            };
            if (string.IsNullOrEmpty(lottery.GiftName))
            {
                lottery.GiftName = null;
                lottery.GiftNum = null;
                lottery.GiftCost = null;
            }
            else
            {
                lottery.GiftCost *= lottery.GiftNum;
            }
            lottery.Time = lottery.EndTime.Add(-lottery.Duration);
            return lottery;
        }
    }
}
