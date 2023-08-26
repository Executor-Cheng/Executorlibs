using Executorlibs.Bilibili.Protocol.Models.General;

namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    public interface ICommonLotteryResultMessage : IBilibiliJsonMessage
    {
        string AwardName { get; }

        uint AwardNum { get; }

        (string, ulong)[] AwardUsers { get; }
    }

    public abstract class CommonLotteryResultMessage : BilibiliJsonMessage, ICommonLotteryResultMessage
    {
        public string AwardName { get; set; } = null!;

        public uint AwardNum { get; set; }

        public (string, ulong)[] AwardUsers { get; set; } = null!;
    }
}
