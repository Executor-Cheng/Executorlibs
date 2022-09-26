using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;
using Executorlibs.Bilibili.Protocol.Models.Enums;
using Executorlibs.Shared;

namespace Executorlibs.Bilibili.Protocol.Parsers
{
    public class UserToastGuardBuyParser : UserToastGuardBuyParser<IGuardBuyMessage, GuardBuyMessage>
    {
        /// <summary>
        /// 初始化 <see cref="UserToastGuardBuyParser"/> 类的新实例
        /// </summary>
        public UserToastGuardBuyParser() { }
    }

    public class UserToastGuardBuyParser<TMessage, TImpl> : BilibiliMappableMessageParser<TMessage> where TMessage : IGuardBuyMessage
                                                                                             where TImpl : GuardBuyMessage, TMessage, new()
    {
        private const string Command = "USER_TOAST_MSG";

        /// <inheritdoc/>
        public override string Key => Command;

        /// <summary>
        /// 初始化 <see cref="UserToastGuardBuyParser{TMessage, TImpl}"/> 类的新实例
        /// </summary>
        public UserToastGuardBuyParser() { }

        /// <inheritdoc/>
        public override bool CanParse(in JsonElement root)
        {
            return root.TryGetProperty("data", out JsonElement data) &&
                   data.TryGetProperty("guard_level", out _) &&
                   data.TryGetProperty("payflow_id", out _);
        }

        /// <summary>
        /// 将给定的 <paramref name="root"/> 处理为 <typeparamref name="TMessage"/> 实例
        /// </summary>
        /// <param name="root">消息数据</param>
        public override TMessage Parse(in JsonElement root)
        {
            JsonElement data = root.GetProperty("data");
            TImpl message = new TImpl
            {
                UserName = data.GetProperty("username").GetString()!,
                UserId = data.GetProperty("uid").GetInt64(),
                Time = Utils.UnixTime2DateTime(data.GetProperty("start_time").GetInt32()),
                GiftCount = data.GetProperty("num").GetInt32(),
                GiftSeedPrice = data.GetProperty("price").GetInt32(),
                GuardType = (GuardType)data.GetProperty("guard_level").GetInt32(),
                OrderId = data.GetProperty("payflow_id").ToString(),
                IsGoldGift = true,
                Rawdata = root
            };
            message.GiftId = (int)message.GuardType + 10000;
            message.GiftName = message.GuardType switch
            {
                GuardType.Captain => "舰长",
                GuardType.Praefect => "提督",
                GuardType.Governor => "总督",
                _ => "未知"
            };
            return message;
        }
    }
}
