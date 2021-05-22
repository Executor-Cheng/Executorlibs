using System;
using System.Text.Json.Serialization;
using Executorlibs.Bilibili.Protocol.Models.General;
using Executorlibs.Shared.JsonConverters;

namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    public interface ICommonLotteryMessage : IBilibiliMessage
    {
        string AwardName { get; }

        int AwardNum { get; }

        string Danmaku { get; }

        TimeSpan Duration { get; }

        DateTime EndTime { get; }

        int? GiftCost { get; }

        string? GiftName { get; }

        int? GiftNum { get; }

        int RequireType { get; }

        int RequireValue { get; }

        DateTime StartTime { get; }
    }

    public abstract class CommonLotteryMessage : BilibiliMessage, ICommonLotteryMessage
    {
        public string Danmaku { get; set; } = null!;

        public string AwardName { get; set; } = null!;

        public int AwardNum { get; set; }

        public virtual string? GiftName { get; set; }

        public virtual int? GiftNum { get; set; }

        public virtual int? GiftCost { get; set; }

        public int RequireType { get; set; }

        public int RequireValue { get; set; }

        [JsonConverter(typeof(UnixTimeStampJsonConverter))]
        public DateTime StartTime => EndTime.Add(-Duration);

        [JsonConverter(typeof(TimeSpanJsonConverter))]
        public TimeSpan Duration { get; set; }

        [JsonConverter(typeof(UnixTimeStampJsonConverter))]
        public DateTime EndTime { get; set; }

        public CommonLotteryMessage() { }

        public CommonLotteryMessage(int id, int roomId, string danmaku, string awardName, int awardNum, string? giftName, int? giftNum, int? giftCost, int requireType, int requireValue, TimeSpan duration, DateTime endTime)
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
