using System;

namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    public interface IDanmakuLotteryMessage : ICommonLotteryMessage
    {

    }

    public class DanmakuLotteryMessage : CommonLotteryMessage, IDanmakuLotteryMessage
    {
        public override uint? GiftCost { get => null; set { } }

        public override string? GiftName { get => null; set { } }

        public override uint? GiftNum { get => null; set { } }

        public DanmakuLotteryMessage() { }

        public DanmakuLotteryMessage(uint id, uint roomId, string danmaku, string awardName, uint awardNum, uint requireType, uint requireValue, TimeSpan duration, DateTime endTime) : base(id, roomId, danmaku, awardName, awardNum, null, null, null, requireType, requireValue, duration, endTime)
        {

        }
    }
}
