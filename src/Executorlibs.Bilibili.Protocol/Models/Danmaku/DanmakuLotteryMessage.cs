using System;

namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    public interface IDanmakuLotteryMessage : ICommonLotteryMessage
    {

    }

    public class DanmakuLotteryMessage : CommonLotteryMessage, IDanmakuLotteryMessage
    {
        public override int? GiftCost { get => null; set { } }

        public override string? GiftName { get => null; set { } }

        public override int? GiftNum { get => null; set { } }

        public DanmakuLotteryMessage() { }

        public DanmakuLotteryMessage(int id, int roomId, string danmaku, string awardName, int awardNum, int requireType, int requireValue, TimeSpan duration, DateTime endTime) : base(id, roomId, danmaku, awardName, awardNum, null, null, null, requireType, requireValue, duration, endTime)
        {

        }
    }
}
