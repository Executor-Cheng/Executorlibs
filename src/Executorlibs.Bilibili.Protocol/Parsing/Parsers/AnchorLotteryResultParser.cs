using Executorlibs.Bilibili.Protocol.Models.Danmaku;

namespace Executorlibs.Bilibili.Protocol.Parsing.Parsers
{
    public class AnchorLotteryResultParser : CommonLotteryResultParser<IAnchorLotteryResultMessage, AnchorLotteryResultMessage>
    {
        private const string Command = "ANCHOR_LOT_AWARD";

        /// <inheritdoc/>
        public override string Key => Command;

        /// <summary>
        /// 初始化 <see cref="AnchorLotteryResultParser"/> 类的新实例
        /// </summary>
        public AnchorLotteryResultParser() { }
    }
}
