using System;
using System.Text.Json.Serialization;
using Executorlibs.Bilibili.Protocol.Models.General;
using Executorlibs.Shared.JsonConverters;

namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    public interface ICommonLotteryMessage : IBilibiliJsonMessage
    {
        string AwardName { get; }

        uint AwardNum { get; }

        string Danmaku { get; }

        TimeSpan Duration { get; }

        DateTime EndTime { get; }

        uint? GiftCost { get; }

        string? GiftName { get; }

        uint? GiftNum { get; }

        uint RequireType { get; }

        uint RequireValue { get; }

        DateTime StartTime { get; }
    }

    public abstract class CommonLotteryMessage : BilibiliJsonMessage, ICommonLotteryMessage
    {
        public string Danmaku { get; set; } = null!;

        public string AwardName { get; set; } = null!;

        public uint AwardNum { get; set; }

        public virtual string? GiftName { get; set; }

        public virtual uint? GiftNum { get; set; }

        public virtual uint? GiftCost { get; set; }

        public uint RequireType { get; set; }

        public uint RequireValue { get; set; }

        [JsonConverter(typeof(UnixTimeStampJsonConverter))]
        public DateTime StartTime => EndTime.Add(-Duration);

        [JsonConverter(typeof(TimeSpanJsonConverter))]
        public TimeSpan Duration { get; set; }

        [JsonConverter(typeof(UnixTimeStampJsonConverter))]
        public DateTime EndTime { get; set; }

        public CommonLotteryMessage() { }

        public CommonLotteryMessage(uint id, uint roomId, string danmaku, string awardName, uint awardNum, string? giftName, uint? giftNum, uint? giftCost, uint requireType, uint requireValue, TimeSpan duration, DateTime endTime)
        {
            Id = id;
            RoomId = roomId;
            Danmaku = danmaku;
            AwardName = awardName;
            AwardNum = awardNum;
            GiftName = giftName;
            GiftNum = giftNum;
            GiftCost = giftCost;
            RequireType = requireType;
            RequireValue = requireValue;
            Duration = duration;
            EndTime = endTime;
        }
    }
}
