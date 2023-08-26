using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;
using Executorlibs.Bilibili.Protocol.Models.Enums;
using Executorlibs.Shared;

namespace Executorlibs.Bilibili.Protocol.Parsing.Parsers
{
    public class UserToastGuardBuyParser : UserToastGuardBuyParser<IUserToastGuardBuyMessage, UserToastGuardBuyMessage>
    {
        /// <summary>
        /// 初始化 <see cref="UserToastGuardBuyParser"/> 类的新实例
        /// </summary>
        public UserToastGuardBuyParser() { }
    }

    public class UserToastGuardBuyParser<TMessage, TImpl> : GuardBuyParser<TMessage, TImpl> where TMessage : IUserToastGuardBuyMessage
                                                                                            where TImpl : UserToastGuardBuyMessage, TMessage, new()
    {
        private const string Command = "USER_TOAST_MSG";

        /// <inheritdoc/>
        public override string Key => Command;

        /// <summary>
        /// 初始化 <see cref="UserToastGuardBuyParser{TMessage, TImpl}"/> 类的新实例
        /// </summary>
        public UserToastGuardBuyParser() { }

        /// <inheritdoc/>
        public override bool CanParse(JsonElement root)
        {
            return root.TryGetProperty("data", out JsonElement data) &&
                   data.TryGetProperty("guard_level", out _) &&
                   data.TryGetProperty("payflow_id", out _);
        }
    }
}
