using Executorlibs.Bilibili.Protocol.Models.General;

namespace Executorlibs.Bilibili.Protocol.Models.Danmaku
{
    public interface ICommonLotteryResultMessage : IBilibiliMessage
    {
        string AwardName { get; }

        int AwardNum { get; }

        (string, int)[] AwardUsers { get; }
    }

    public abstract class CommonLotteryResultMessage : BilibiliMessage, ICommonLotteryResultMessage
    {
        public string AwardName { get; set; } = null!;

        public int AwardNum { get; set; }

        public (string, int)[] AwardUsers { get; set; } = null!;
    }
}
