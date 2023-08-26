using System;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;
using Executorlibs.Shared;

namespace Executorlibs.Bilibili.Protocol.Parsing.Parsers
{
    public class DanmakuLotteryParser : DanmakuLotteryParser<IDanmakuLotteryMessage, DanmakuLotteryMessage>
    {
        /// <summary>
        /// 初始化 <see cref="DanmakuLotteryParser"/> 类的新实例
        /// </summary>
        public DanmakuLotteryParser() { }
    }

    public class DanmakuLotteryParser<TMessage, TImpl> : BilibiliMappableJsonMessageParser<TMessage, TImpl> where TMessage : IDanmakuLotteryMessage
                                                                                                            where TImpl : DanmakuLotteryMessage, TMessage, new()
    {
        private const string Command = "DANMU_GIFT_LOTTERY_START";

        /// <inheritdoc/>
        public override string Key => Command;

        /// <summary>
        /// 初始化 <see cref="DanmakuLotteryParser{TMessage, TImpl}"/> 类的新实例
        /// </summary>
        public DanmakuLotteryParser() { }

        protected override TImpl CreateMessage(JsonElement rawdata)
        {
            var message = base.CreateMessage(rawdata);
            var data = rawdata.GetProperty("data");
            message.Id = data.GetProperty("id").GetUInt32();
            message.Danmaku = data.GetProperty("danmu").GetString()!;
            message.AwardName = data.GetProperty("award_name").GetString()!;
            message.AwardNum = data.GetProperty("award_num").GetUInt32();
            message.RequireType = data.GetProperty("require_type").GetUInt32();
            message.RequireValue = data.GetProperty("require_value").GetUInt32();
            message.Duration = TimeSpan.FromSeconds(data.GetProperty("max_time").GetInt32());
            message.EndTime = Utils.UnixTime2DateTime(data.GetProperty("current_time").GetInt32()).AddSeconds(data.GetProperty("time").GetInt32());
            message.Time = message.EndTime.Add(-message.Duration);
            return message;
        }
    }
}
