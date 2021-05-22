using Executorlibs.Bilibili.Protocol.Models.Danmaku;

namespace Executorlibs.Bilibili.Protocol.Parsers
{
    public class DanmakuLotteryResultParser : CommonLotteryResultParser<IDanmakuLotteryResultMessage, DanmakuLotteryResultMessage>
    {
        private const string Command = "DANMU_GIFT_LOTTERY_AWARD";

        /// <inheritdoc/>
        public override string Key => Command;

        /// <summary>
        /// 初始化 <see cref="DanmakuLotteryResultParser"/> 类的新实例
        /// </summary>
        public DanmakuLotteryResultParser() { }
    }
}
